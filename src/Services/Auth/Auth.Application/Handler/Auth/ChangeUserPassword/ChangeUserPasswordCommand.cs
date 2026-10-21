using Auth.Application.Dtos.Input;
using FluentValidation;

namespace Auth.Application.Handler.Auth.ChangeUserPassword;

public record ChangeUserPasswordCommand(ChangePasswordInput Reset) : ICommand<ChangeUserPasswordResult>;

public record ChangeUserPasswordResult(bool IsSuccess);

public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(x => x.Reset)
            .NotNull()
            .WithMessage("El objeto Reset es obligatorio")
            .NotEmpty()
            .WithMessage("El objeto Reset no puede estar vacío");
        RuleFor(x => x.Reset.NewPassword).NotNull()
            .WithMessage("El campo NewPassword es obligatorio")
            .NotEmpty()
            .WithMessage("El campo NewPassword no puede estar vacío")
            .MinimumLength(8)
            .WithMessage("La contraseña debe tener al menos 8 caracteres")
            .MaximumLength(128)
            .WithMessage("La contraseña no puede superar los 128 caracteres")
            .Matches("[A-Z]")
            .WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches("[a-z]")
            .WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches("[0-9]")
            .WithMessage("La contraseña debe contener al menos un número");
        RuleFor(x => x.Reset.ResetJwt).NotNull()
            .WithMessage("El campo ResetJwt es obligatorio")
            .NotEmpty()
            .WithMessage("El campo ResetJwt no puede estar vacío")
            .MaximumLength(2048)
            .WithMessage("El campo ResetJwt no es válido");
    }
}
