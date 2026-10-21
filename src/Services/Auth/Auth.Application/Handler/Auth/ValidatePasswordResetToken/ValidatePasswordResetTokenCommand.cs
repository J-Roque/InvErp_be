using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using FluentValidation;

namespace Auth.Application.Handler.Auth.ValidatePasswordResetToken;

public record ValidatePasswordResetTokenCommand(ValidatePasswordResetTokenInput Reset)
    : ICommand<ValidatePasswordResetTokenResult>;

public record ValidatePasswordResetTokenResult(ValidatePasswordResetTokenMessage Result);

public class ValidatePasswordResetTokenCommandValidator : AbstractValidator<ValidatePasswordResetTokenCommand>
{
    public ValidatePasswordResetTokenCommandValidator()
    {
        RuleFor(x => x.Reset).NotNull()
            .WithMessage("El objeto Reset es obligatorio");
        RuleFor(x => x.Reset.ResetJwt).NotNull()
            .WithMessage("El campo ResetJwt es obligatorio")
            .NotEmpty()
            .WithMessage("El campo ResetJwt no puede estar vacío")
            .MaximumLength(2048)
            .WithMessage("El campo ResetJwt no es válido");
    }
}
