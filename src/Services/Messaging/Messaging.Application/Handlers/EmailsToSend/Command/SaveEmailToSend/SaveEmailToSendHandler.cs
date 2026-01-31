using Messaging.Application.Dtos.General;

namespace Messaging.Application.Handlers.EmailsToSend.Command.SaveEmailToSend;

public class SaveEmailToSendHandler(IApplicationDbContext dbContext)
    : ICommandHandler<SaveEmailToSendCommand, SaveEmailToSendResult>
{
    public async Task<SaveEmailToSendResult> Handle(SaveEmailToSendCommand command, CancellationToken cancellationToken)
    {
        var emailToSend = CreateNewEmailToSend(command.Email);
        dbContext.EmailsToSend.Add(emailToSend);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SaveEmailToSendResult(emailToSend.Id);
    }

    private EmailToSend CreateNewEmailToSend(EmailToSendDto emailToSendDto)
    {
        var newEmailToSend = EmailToSend.Create(
            subject: emailToSendDto.Subject,
            template: emailToSendDto.Template,
            data: emailToSendDto.Data,
            recipients: emailToSendDto.Recipients,
            cc: emailToSendDto.Cc,
            bcc: emailToSendDto.Bcc,
            priority: emailToSendDto.Priority
        );

        return newEmailToSend;
    }
}