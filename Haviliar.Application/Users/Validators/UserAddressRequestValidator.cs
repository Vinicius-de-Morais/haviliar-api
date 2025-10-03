using FluentValidation;
using Haviliar.DataTransfer.Users.Requests;

namespace Haviliar.Application.Users.Validators;

public class UserAddressRequestValidator : AbstractValidator<UserAddressRequest>
{
    public UserAddressRequestValidator()
    {
        RuleFor(x => x.City)
             .NotEmpty()
             .WithMessage("Cidade não pode ser vazia.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("Estado não pode ser vazio.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .WithMessage("Bairro não pode ser vazio.");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("CEP não pode ser vazio.")
            .Matches(@"^\d{5}-?\d{3}$")
            .WithMessage("CEP deve estar no formato 00000-000.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Rua não pode ser vazia.");

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Número não pode ser vazio.")
            .Matches(@"^\d+[A-Za-z]?$").WithMessage("Número deve ser numérico (pode conter complemento como 'A').");

        RuleFor(x => x.Complement)
            .MaximumLength(150).WithMessage("Complemento não pode ter mais de 150 caracteres.");
    }
}
