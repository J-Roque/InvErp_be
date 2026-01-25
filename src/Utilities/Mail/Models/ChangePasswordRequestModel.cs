namespace Mail.Models;

public class ChangePasswordRequestModel
{
    public string Username { get; set; } = "";
    public string RequestDate { get; set; } = "";
    public string Jwt { get; set; } = "";
    public string ResetUrl { get; set; } = "";
}