
namespace Common.Interfaces.Dtos;
public interface IUserContextDto
{
    public long UserId { get; }
    public long ProfileId { get; }
    public List<long> Roles { get; }
    public string? Email { get; }
    public string? Ip { get; }
    public long? TokenId { get; }
}
