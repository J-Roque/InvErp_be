using Messaging.Application.RemoteServices.Modules.Messaging.Models;

namespace Messaging.Application.RemoteServices.Modules.Messaging.Interfaces;

public interface IMessagingRemoteService
{
    Task<SaveEmailToSendResult> SaveEmailToSend(EmailToSendBody body);
    Task<SendEmailResult> SendEmail(EmailToSendBody body);
}