namespace Mail.Models;

public class NewUserCreatedModel
{
    public string Subject { get; set; } = "";
    public string User { get; set; } = "";
    public string CreationDate { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}