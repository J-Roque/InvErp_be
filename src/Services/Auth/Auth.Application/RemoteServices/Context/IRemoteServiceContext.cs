using Auth.Application.RemoteServices.Modules.Attachments.Interfaces;
using Auth.Application.RemoteServices.Modules.Auth.Interfaces;
using Auth.Application.RemoteServices.Modules.Event.Interfaces;
using Auth.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Auth.Application.RemoteServices.Context;

public interface IRemoteServiceContext
{
    IAuthRemoteService Auth { get; }
    IEventRemoteService Event { get; }
    IMessagingRemoteService Messaging { get; }
    IAttachmentRemoteService Attachment { get; }


}
