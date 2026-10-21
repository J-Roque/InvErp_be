using Security.Application.RemoteServices.Modules.Attachments.Models;

namespace Security.Application.RemoteServices.Modules.Attachments.Interfaces;

public interface IAttachmentRemoteService
{
    Task<SignFileAndUpdateResult> SignFileAndUpdate(SignFileAndUpdateBody body);
}