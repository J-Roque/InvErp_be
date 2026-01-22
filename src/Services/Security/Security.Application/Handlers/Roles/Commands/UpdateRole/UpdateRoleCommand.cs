using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;

namespace Security.Application.Handlers.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(long Id, RoleDto Role, UserContextDto UserContext) : ICommand<UpdateRoleResult>;

public record UpdateRoleResult(long Id);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del rol debe ser mayor a cero");
        RuleFor(x => x.Role).NotEmpty().WithMessage("El objeto Role es obligatorio");
        RuleFor(x => x.Role.Name).NotEmpty().WithMessage("El campo Name es obligatorio");
        RuleFor(x => x.Role.IsActive)
            .NotNull().WithMessage("El campo IsActive es obligatorio");
        RuleFor(x => x.Role.IdentityStatusId)
            .NotNull().WithMessage("El campo IdentityStatusId es obligatorio")
            .IsInEnum().WithMessage("El campo IdentityStatusId debe ser un valor valido.");
    }
}
