using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;

namespace Security.Application.Handlers.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(long Id, UserContextDto UserContext) : ICommand<DeleteRoleResult>;

public record DeleteRoleResult(bool Success);

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del rol debe ser mayor a cero");
    }
}
