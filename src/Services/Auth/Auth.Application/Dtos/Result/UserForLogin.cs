using System.Text.Json;
using Auth.Application.Dtos.General;

namespace Auth.Application.Dtos.Result;

public class UserForLoginSp
{
    public long UserId { get; set; }
    public string FullName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Salt { get; set; } = "";
    public string? Email { get; set; }
    public string? Roles { get; set; }
    public string? Profile { get; set; }
}

public class UserForLogin : UserForLoginSp
{
    public new List<RoleDto>? Roles { get; set; }
    public new ProfileDto? Profile { get; set; }

    public UserForLogin(UserForLoginSp userForLoginSp)
    {
        UserId = userForLoginSp.UserId;
        FullName = userForLoginSp.FullName;
        Username = userForLoginSp.Username;
        Password = userForLoginSp.Password;
        Salt = userForLoginSp.Salt;
        Email = userForLoginSp.Email;

        Roles = userForLoginSp.Roles != null
            ? JsonSerializer.Deserialize<List<RoleDto>>(userForLoginSp.Roles)
            : [];

        Profile = userForLoginSp.Profile != null
            ? JsonSerializer.Deserialize<ProfileDto>(userForLoginSp.Profile)
            : null;
    }
}
