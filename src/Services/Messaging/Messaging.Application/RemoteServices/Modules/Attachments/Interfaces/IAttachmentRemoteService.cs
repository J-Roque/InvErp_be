using Messaging.Application.RemoteServices.Modules.Attachments.Models;

namespace Messaging.Application.RemoteServices.Modules.Attachments.Interfaces;

public interface IAttachmentRemoteService
{
    Task<SignFileAndUpdateResult> SignFileAndUpdate(SignFileAndUpdateBody body);
}