using Mail.Services;
using Messaging.Application.Configuraction;
using Messaging.Application.Configuration;

namespace Messaging.Application.Utilities;

public class MailSenderUtility
{
    private readonly MailSender _mailSender;

    public MailSenderUtility(AppConstants appConstants)
    {
        _mailSender = new MailSender(
            host: appConstants.Mail.Host,
            port: appConstants.Mail.Port,
            from: appConstants.Mail.From,
            user: appConstants.Mail.User,
            password: appConstants.Mail.Password,
            enableSsl: appConstants.Mail.EnableSsl
        );
    }

    public MailSender Instance => _mailSender;
    
}