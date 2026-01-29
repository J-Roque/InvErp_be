using Auth.Application.RemoteServices.Modules.Attachments.Interfaces;
using Auth.Application.RemoteServices.Modules.Auth.Interfaces;
using Auth.Application.RemoteServices.Modules.Event.Interfaces;
using Auth.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Auth.Application.RemoteServices.Context;

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