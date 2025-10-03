using FluentValidation;
using Haviliar.DataTransfer.Users.Requests;
using Haviliar.Infra.Utils;

namespace Haviliar.Application.Users.Validators;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("Nome não pode ser vazio.")
               .MaximumLength(255)
               .WithMessage("O nome não pode conter mais de 255 caracteres");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Data de nascimento não pode ser vazia.")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Data de nascimento deve ser anterior ou igual a data atual.");


        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("E-mail não pode ser vazio.")
            .EmailAddress()
            .WithMessage("E-mail inválido.")
            .MaximumLength(255)
            .WithMessage("O e-mail não pode conter mais de 255 caracteres");

        RuleFor(x => x.UserType)
            .IsInEnum()
            .WithMessage("Tipo de usuário inválido.");

        RuleFor(x => x.Document)
            .MustBeCpf()
            .WithMessage("O CPF deve ser válido.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Telefone não pode ser vazio.")
            .MustBePhoneNumber()
            .WithMessage("Telefone deve ser um celular válido com DDD (ex: 41991468403).");

        RuleFor(x => x.UserAddressRequest)
            .NotNull()
            .WithMessage("Endereço do usuário é obrigatório.")
            .SetValidator(new UserAddressRequestValidator());

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha não pode ser vazia.")
            .MinimumLength(6)
            .WithMessage("Senha deve conter no mínimo 6 caracteres.")
            .MaximumLength(255)
            .WithMessage("Senha deve conter no máximo 255 caracteres.")
            .Matches("[A-Z]")
            .WithMessage("Senha deve conter ao menos uma letra maiúscula.")
            .Matches("[a-z]")
            .WithMessage("Senha deve conter ao menos uma letra minúscula.")
            .Matches("[0-9]")
            .WithMessage("Senha deve conter ao menos um número.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Senha deve conter ao menos um caractere especial.");
    }
}
