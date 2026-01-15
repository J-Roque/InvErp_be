using BuildingBlocks.CQRS;
using Security.Application.Dtos.Results;

namespace Security.Application.Handlers.Users.Queries.GetUserById;

public record GetUserByIdQuery(long Id) : IQuery<GetUserByIdResult>;

public record GetUserByIdResult(UserInfo User);
