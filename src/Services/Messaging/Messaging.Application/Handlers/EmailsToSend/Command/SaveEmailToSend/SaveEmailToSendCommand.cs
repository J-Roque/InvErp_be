using FluentValidation;
using Messaging.Application.Dtos.General;

namespace Messaging.Application.Handlers.EmailsToSend.Command.SaveEmailToSend;

public record SaveEmailToSendCommand(EmailToSendDto Email) : ICommand<SaveEmailToSendResult>;

public record SaveEmailToSendResult(long Id);

public class SaveEmailToSendCommandValidator : AbstractValidator<SaveEmailToSendCommand>
{
    public SaveEmailToSendCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("El objeto Email es obligatorio");
        RuleFor(x => x.Email.Subject).NotEmpty().WithMessage("El campo Subject es obligatorio");
        RuleFor(x => x.Email.Template).NotEmpty().WithMessage("El campo Template es obligatorio");
        RuleFor(x => x.Email.Data).NotEmpty().WithMessage("El campo Data es obligatorio");
        RuleFor(x => x.Email.Recipients).NotEmpty().WithMessage("El campo Recipients es obligatorio");
        RuleFor(x => x.Email.Priority).NotEmpty().WithMessage("El campo Priority es obligatorio");
    }
}
