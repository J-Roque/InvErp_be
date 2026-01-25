namespace Auth.Application.RemoteServices.Modules.Attachments.Models;

public class SignFileAndUpdateBody
{
    public long Id { get; set; }
    public required string OriginalFileName { get; set; }
    public required string NewFileName { get; set; }
    public long Size { get; set; }
    public required string ContentType { get; set; }
    public required string Extension { get; set; }
    public byte[] Data { get; set; } = [];
    public required string Url { get; set; }
}
public class SignFileAndUpdateResult
{
    public bool IsSuccess { get; set; }
    public string Url { get; set; } = "";
}

