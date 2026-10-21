using Security.Application.RemoteServices.Modules.Attachments.Interfaces;
using Security.Application.RemoteServices.Modules.Auth.Interfaces;
using Security.Application.RemoteServices.Modules.Event.Interfaces;
using Security.Application.RemoteServices.Modules.Messaging.Interfaces;

namespace Security.Application.RemoteServices.Context;

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