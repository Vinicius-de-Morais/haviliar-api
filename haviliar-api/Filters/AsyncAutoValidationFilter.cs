using FluentValidation;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace haviliar_api.Filters;

public class AsyncAutoValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AsyncAutoValidationFilter(IServiceProvider serviceProvider, ProblemDetailsFactory problemDetailsFactory)
    {
        _serviceProvider = serviceProvider;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (ParameterDescriptor parameter in context.ActionDescriptor.Parameters)
        {
            if (parameter.BindingInfo!.BindingSource == BindingSource.Body || (parameter.BindingInfo.BindingSource == BindingSource.Query && parameter.ParameterType.IsClass))
            {
                if (_serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(parameter.ParameterType)) is not
                    IValidator validator)
                {
                    continue;
                }

                object? subject = context.ActionArguments[parameter.Name];

                ValidationResult? result = await validator.ValidateAsync(new ValidationContext<object>(subject!), context.HttpContext.RequestAborted);
                if (!result.IsValid)
                {
                    result.Errors.ToList().ForEach(e => context.ModelState.AddModelError(e.PropertyName, e.ErrorMessage));
                }
            }
        }

        if (!context.ModelState.IsValid)
        {
            ValidationProblemDetails validationProblem = _problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext,
                context.ModelState,
                statusCode: null,
                title: null,
                type: null,
                detail: null,
                instance: null);

            context.Result = new BadRequestObjectResult(validationProblem);
        }
        else
        {
            await next();
        }
    }
}
