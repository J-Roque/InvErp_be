namespace Messaging.Application.RemoteServices.Modules.Event.Models;

public class SaveEventBody
{
    public string? ActivityDescription { get; set; }
    public string? IpAddress { get; set; }
    public string? ModuleName { get; set; }
    public long EventTypeId { get; set; }
    public long UserId { get; set; }
    public string? Data { get; set; }
    public long? TokenId { get; set; }
}

public class SaveEventResult
{
    public bool IsSuccess { get; set; }
}