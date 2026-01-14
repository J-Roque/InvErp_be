namespace Security.Application.Dtos.General;

public record ProfileDto
(
    long? Id,
    string Name,
    bool IsActive,
    List<long> NavigationItemIds
);
