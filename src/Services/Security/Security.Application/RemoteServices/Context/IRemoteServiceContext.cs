using Security.Application.RemoteServices.Modules.Attachments.Interfaces;
using Security.Application.RemoteServices.Modules.Auth.Interfaces;
using Security.Application.RemoteServices.Modules.Event.Interfaces;
using Security.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Security.Application.RemoteServices.Context;

public interface IRemoteServiceContext
{
    IAuthRemoteService Auth { get; }
    IEventRemoteService Event { get; }
    IMessagingRemoteService Messaging { get; }
    IAttachmentRemoteService Attachment { get; }
}