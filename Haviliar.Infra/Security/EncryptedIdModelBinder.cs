using Haviliar.Domain.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Haviliar.Infra.Security;

public class EncryptedIdModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        string? value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("O identificador (ID) criptografado não foi informado.");
        }

        try
        {
            EncryptedInt decrypted = IdCryptService.DecryptId(value);
            bindingContext.Result = ModelBindingResult.Success(decrypted);
        }
        catch
        {
            throw new DomainException("O identificador (ID) criptografado é inválido ou está corrompido.");
        }

        return Task.CompletedTask;
    }
}
