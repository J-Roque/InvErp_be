using Auth.Application.Dtos.Result;

namespace Auth.Application.Interfaces.Persistence;

public interface IAuthRepository
{
    public Task<UserForLogin?> SearchUserForLogin(string value);

    public Task<bool> ChangePassword(long userId, string newPassword, string salt, string tokenCode);

    public Task<JwtDataInfo?> GetJwtDataInfo(string jwt);

}
