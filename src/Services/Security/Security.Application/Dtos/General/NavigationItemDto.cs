namespace Security.Application.Dtos.General;

public record NavigationItemDto
(
    long Id,
    string Module,
    string Name,
    string? Route,
    string? Icon,
    long? ParentId,
    int? OrderIndex,
    int? Level,
    bool ShowInMenu,
    List<NavigationItemDto> Children
);
