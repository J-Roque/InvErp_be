using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;

namespace Security.Application.Handlers.Roles.Commands.CreateRole;

public record CreateRoleCommand(RoleDto Role, UserContextDto UserContext) : ICommand<CreateRoleResult>;

public record CreateRoleResult(long Id);

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Role).NotEmpty().WithMessage("El objeto Role es obligatorio");
        RuleFor(x => x.Role.Name).NotEmpty().WithMessage("El campo Name es obligatorio");
        RuleFor(x => x.Role.IsActive)
            .NotNull().WithMessage("El campo IsActive es obligatorio")
            .Must(value => value == true || value == false).WithMessage("El campo IsActive debe ser un booleano");
        RuleFor(x => x.Role.IdentityStatusId)
            .NotNull().WithMessage("El campo IdentityStatusId es obligatorio")
            .IsInEnum().WithMessage("El campo IdentityStatusId debe ser un valor valido.");
    }
}
