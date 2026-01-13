using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Auth.Application.Handler.Auth.Login;

public record LoginCommand(LoginInput Login, string? Ip, string? Device, string? UserAgent): ICommand<LoginResult>;
public record LoginResult(LoginAuth LoginAuth);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("El objeto Login es obligatorio");
        RuleFor(x => x.Login.Username).NotEmpty().WithMessage("El campo Username es obligatorio");
        RuleFor(x => x.Login.Password).NotEmpty().WithMessage("El campo Password es obligatorio");
    }
}
