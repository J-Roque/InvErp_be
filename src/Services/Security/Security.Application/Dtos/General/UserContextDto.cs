using Common.Interfaces.Dtos;

namespace Security.Application.Dtos.General;

public class UserContextDto(long userId, long profileId, List<long> roles, string? email, string? ip, long? tokenId)
    : IUserContextDto
{
    public long UserId { get; } = userId;
    public long ProfileId { get; } = profileId;
    public List<long> Roles { get; } = roles;
    public string? Email { get; } = email;
    public string? Ip { get; } = ip;
    public long? TokenId { get; } = tokenId;

}