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
            .WithMessage("El campo NewPassword no puede estar vacío");
        RuleFor(x => x.Reset.ResetJwt).NotNull()
            .WithMessage("El campo ResetJwt es obligatorio")
            .NotEmpty()
            .WithMessage("El campo ResetJwt no puede estar vacío");
    }
}
