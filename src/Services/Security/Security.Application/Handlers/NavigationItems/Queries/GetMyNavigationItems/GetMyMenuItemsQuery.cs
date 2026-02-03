using Security.Application.Dtos.General;

namespace Security.Application.Handlers.NavigationItems.Queries.GetMyNavigationItems;



public record GetMyNavigationItemsQuery(long UserId, long ProfileId) : IQuery<GetMyNavigationItemsResult>;
public record GetMyNavigationItemsResult(List<NavigationItemDto> NavigationItems);