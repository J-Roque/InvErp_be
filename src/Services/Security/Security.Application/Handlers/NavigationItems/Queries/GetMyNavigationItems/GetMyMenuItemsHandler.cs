using BuildingBlocks.Exceptions;
using Security.Application.Exceptions;
using Security.Application.Extensions;
using Security.Application.Interfaces.Persistance;

namespace Security.Application.Handlers.NavigationItems.Queries.GetMyNavigationItems;

public class GetMyMenuItemsHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork)
: IQueryHandler<GetMyNavigationItemsQuery, GetMyNavigationItemsResult>
{

    public async Task<GetMyNavigationItemsResult> Handle(GetMyNavigationItemsQuery query, CancellationToken cancellationToken)
    {
        var userId = query.UserId;
        var profileId = query.ProfileId;

        var profileInfo = await unitOfWork.Profiles.GetProfileInfoById(profileId);

        if (profileInfo == null)
        {
            throw new ProfileNotFoundException(profileId, $"Perfil con Id {profileId} no encontrado");
        }

        var menuItemsId = profileInfo.NavigationItemIds;

        if (menuItemsId == null || menuItemsId?.Count == 0)
        {
            throw new BadRequestException($"El perfil asociado al usuario {userId} no tiene items de navegación asignados");
        }

        var menuItems = await dbContext.NavigationItems
            .Where(m => menuItemsId != null && menuItemsId.Contains(m.Id))
            .OrderBy(m => m.Level)
            .ThenBy(m => m.OrderIndex)
            .ToListAsync(cancellationToken);

        var menuItemHierarchy = menuItems.BuildNavigationHierarchy();

        return new GetMyNavigationItemsResult(menuItemHierarchy.ToNavigationItemDtoList().ToList());
    }



}