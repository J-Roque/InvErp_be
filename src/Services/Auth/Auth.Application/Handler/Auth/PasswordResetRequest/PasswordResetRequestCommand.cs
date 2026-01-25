
using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using FluentValidation;


namespace Auth.Application.Handler.Auth.PasswordResetRequest;

public record PasswordResetRequestCommand(PasswordResetRequestInput Reset) : ICommand<PasswordResetRequestResult>;

public record PasswordResetRequestResult(ResetRequestMessage Result);
public class PasswordResetRequestCommandValidator : AbstractValidator<PasswordResetRequestCommand>
{
    public PasswordResetRequestCommandValidator()
    {
        RuleFor(x => x.Reset)
            .NotEmpty().WithMessage("El objeto Reset es obligatorio");

        RuleFor(x => x.Reset.Type)
            .NotEmpty().WithMessage("El campo Type es obligatorio")
            .Must(type => type is "email" or "username")
            .WithMessage("El campo Type solo acepta los valores 'email' o 'username'");

        RuleFor(x => x.Reset.Value)
            .NotEmpty().WithMessage("El campo Value es obligatorio")
            .DependentRules(() =>
            {
                RuleFor(x => x.Reset)
                    .Must(x => x.Type != "email" || IsValidEmail(x.Value))
                    .WithMessage("El campo Value debe ser un correo electrónico válido si el Type es 'email'");
            });
    }

    private static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) &&
               new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
    }

}