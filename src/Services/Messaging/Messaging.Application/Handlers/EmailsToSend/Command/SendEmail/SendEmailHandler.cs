using Mail.Services;
using Messaging.Application.Utilities;

namespace Messaging.Application.Handlers.EmailsToSend.Command.SendEmail;

public class SendEmailHandler(IApplicationDbContext dbContext, MailSenderUtility mailSenderUtility)
    : ICommandHandler<SendEmailCommand, SendEmailResult>
{
    public async Task<SendEmailResult> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var toSend = request.Email;
        var data = toSend.Data;
        var templateType = toSend.Template;

        var htmlBody = await MailService.GetTemplateData(templateType, data);

        var isSent = await mailSenderUtility.Instance.SendEmailAsync(
            to: toSend.Recipients,
            subject: toSend.Subject,
            htmlBody: htmlBody
        );

        return new SendEmailResult(isSent);
    }
}