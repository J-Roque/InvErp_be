using BuildingBlocks.CQRS;
using Security.Application.Dtos.Results;

namespace Security.Application.Handlers.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(long Id) : IQuery<GetRoleByIdResult>;

public record GetRoleByIdResult(RoleInfo Role);
