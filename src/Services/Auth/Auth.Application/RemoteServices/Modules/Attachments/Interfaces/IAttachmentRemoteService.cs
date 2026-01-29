using Auth.Application.RemoteServices.Modules.Attachments.Models;

namespace Auth.Application.RemoteServices.Modules.Attachments.Interfaces;

public interface IAttachmentRemoteService
{
    Task<SignFileAndUpdateResult> SignFileAndUpdate(SignFileAndUpdateBody body);
}