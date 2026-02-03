using Security.Application.Dtos.General;

namespace Security.Application.Extensions;

public static class NavigationItemExtension
{

    public static IEnumerable<NavigationItemDto> ToNavigationItemDtoList(this IEnumerable<NavigationItem> navigationItems)
    {
        return navigationItems.Select((navigationItem) => new NavigationItemDto(
            Id: navigationItem.Id,
            Module: navigationItem.Module,
            Name: navigationItem.Name,
            Route: navigationItem.Route,
            Icon: navigationItem.Icon,
            ParentId: navigationItem.ParentId,
            OrderIndex: navigationItem.OrderIndex,
            Level: navigationItem.Level,
            ShowInMenu: navigationItem.ShowInMenu,
            Children: ToNavigationItemDtoList(navigationItem.Children).ToList()
        ));
    }

    public static NavigationItemDto ToNavigationItemDto(this NavigationItem navigationItem)
    {
        return new NavigationItemDto(
            Id: navigationItem.Id,
            Module: navigationItem.Module,
            Name: navigationItem.Name,
            Route: navigationItem.Route,
            Icon: navigationItem.Icon,
            ParentId: navigationItem.ParentId,
            OrderIndex: navigationItem.OrderIndex,
            Level: navigationItem.Level,
            ShowInMenu: navigationItem.ShowInMenu,
            Children: ToNavigationItemDtoList(navigationItem.Children).ToList()
        );
    }

    public static List<NavigationItem> BuildNavigationHierarchy(this List<NavigationItem> menuItems)
    {
        var groupedItems = menuItems
            .GroupBy(m => m.ParentId ?? 0)
            .ToDictionary(g => g.Key, g => g.ToList());

        var rootItems = groupedItems.ContainsKey(0) ? groupedItems[0] : new List<NavigationItem>();

        void AssignChildren(NavigationItem parent)
        {
            if (groupedItems.ContainsKey(parent.Id))
            {
                parent.Children = groupedItems[parent.Id];
                foreach (var child in parent.Children)
                {
                    AssignChildren(child);
                }
            }
            else
            {
                parent.Children = new List<NavigationItem>();
            }
        }

        foreach (var root in rootItems)
        {
            AssignChildren(root);
        }

        return rootItems;
    }

}