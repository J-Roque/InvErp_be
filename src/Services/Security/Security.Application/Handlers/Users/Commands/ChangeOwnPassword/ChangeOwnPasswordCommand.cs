using Security.Application.Dtos.General;
using Security.Application.Dtos.Input;

namespace Security.Application.Handlers.Users.Commands.ChangeOwnPassword;

public record ChangeOwnPasswordCommand(ChangeOwnPasswordInput User, UserContextDto UserContext) : IRequest<ChangeOwnPasswordResult>, ICommand<ChangeOwnPasswordResult>;

public record ChangeOwnPasswordResult(bool IsSuccess);

public class ChangeOwnPasswordCommandValidator : AbstractValidator<ChangeOwnPasswordCommand>
{
    public ChangeOwnPasswordCommandValidator()
    {
        RuleFor(x => x.User.OldPassword).NotEmpty().WithMessage("El campo OldPassword es obligatorio");
        RuleFor(x => x.User.NewPassword).NotEmpty().WithMessage("El campo NewPassword es obligatorio");
    }
}