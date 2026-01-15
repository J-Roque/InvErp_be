using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;

namespace Security.Application.Handlers.Users.Commands.DeleteUser;

public class DeleteUserHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteUserCommand, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("Usuario", command.Id);
        }

        // Soft delete - cambia el estado a Inactive
        user.Delete();

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteUserResult(true);
    }
}
