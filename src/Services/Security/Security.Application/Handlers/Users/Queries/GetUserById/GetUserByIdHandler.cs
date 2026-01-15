using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Security.Application.Interfaces.Persistance;

namespace Security.Application.Handlers.Users.Queries.GetUserById;

public class GetUserByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserInfoById(query.Id);

        if (user == null)
        {
            throw new NotFoundException("Usuario", query.Id);
        }

        return new GetUserByIdResult(user);
    }
}
