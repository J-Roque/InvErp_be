using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;

namespace Security.Application.Handlers.Users.Commands.DeleteUser;

public record DeleteUserCommand(long Id, UserContextDto UserContext) : ICommand<DeleteUserResult>;

public record DeleteUserResult(bool Success);

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del usuario debe ser mayor a cero");
    }
}
