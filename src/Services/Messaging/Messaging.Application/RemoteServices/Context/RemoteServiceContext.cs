using Messaging.Application.RemoteServices.Modules.Attachments.Interfaces;
using Messaging.Application.RemoteServices.Modules.Auth.Interfaces;
using Messaging.Application.RemoteServices.Modules.Event.Interfaces;
using Messaging.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Messaging.Application.RemoteServices.Context;

public class RemoteServiceContext(
    IAuthRemoteService authRemoteService,
    IEventRemoteService eventRemoteService,
    IMessagingRemoteService messagingRemoteService,
    IAttachmentRemoteService attachmentRemoteService
) : IRemoteServiceContext
{
    public IAuthRemoteService Auth => authRemoteService;
    public IEventRemoteService Event => eventRemoteService;
    public IMessagingRemoteService Messaging => messagingRemoteService;
    public IAttachmentRemoteService Attachment => attachmentRemoteService;
}