using Auth.Application.Dtos.General;
using FluentValidation;

namespace Auth.Application.Handler.Auth.ValidatePrivateJwt;

public record ValidatePrivateJwtCommand(string Jwt) : ICommand<ValidatePrivateJwtResult>;

public record ValidatePrivateJwtResult(
    bool IsValid,
    string Message,
    long? UserId,
    string? Email,
    List<RoleDto>? Roles,
    ProfileDto? Profile,
    string? Ip,
    long? TokenId
);

public class ValidatePrivateJwtCommandValidator : AbstractValidator<ValidatePrivateJwtCommand>
{
    public ValidatePrivateJwtCommandValidator()
    {
        RuleFor(x => x.Jwt).NotEmpty()
            .WithMessage("El campo Jwt no puede estar vacío")
            .MaximumLength(4096)
            .WithMessage("El campo Jwt no es válido");
    }
}
