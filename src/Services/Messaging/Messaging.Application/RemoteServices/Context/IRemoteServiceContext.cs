using Messaging.Application.RemoteServices.Modules.Attachments.Interfaces;
using Messaging.Application.RemoteServices.Modules.Auth.Interfaces;
using Messaging.Application.RemoteServices.Modules.Event.Interfaces;
using Messaging.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Messaging.Application.RemoteServices.Context;

public interface IRemoteServiceContext
{
    IAuthRemoteService Auth { get; }
    IEventRemoteService Event { get; }
    IMessagingRemoteService Messaging { get; }
    IAttachmentRemoteService Attachment { get; }
}