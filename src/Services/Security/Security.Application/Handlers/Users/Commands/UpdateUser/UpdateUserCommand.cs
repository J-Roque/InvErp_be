using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;
using Security.Application.Dtos.Input;

namespace Security.Application.Handlers.Users.Commands.UpdateUser;

public record UpdateUserCommand(long Id, UpdateUserInput User, UserContextDto UserContext) : ICommand<UpdateUserResult>;

public record UpdateUserResult(long Id);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del usuario debe ser mayor a cero");
        RuleFor(x => x.User).NotEmpty().WithMessage("El objeto User es obligatorio");
        RuleFor(x => x.User.Email).NotEmpty().WithMessage("El campo Email es obligatorio");
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("El campo FirstName es obligatorio");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("El campo LastName es obligatorio");
        RuleFor(x => x.User.Username).NotEmpty().WithMessage("El campo Username es obligatorio");
        RuleFor(x => x.User.DocumentTypeId)
            .NotEmpty().WithMessage("El campo DocumentTypeId es obligatorio")
            .GreaterThan(0).WithMessage("El campo DocumentTypeId debe ser un número mayor a cero");
        RuleFor(x => x.User.DocumentNumber).NotEmpty().WithMessage("El campo DocumentNumber es obligatorio");
        RuleFor(x => x.User.IdentityStatusId).NotEmpty().WithMessage("El campo IdentityStatusId es obligatorio");
        RuleFor(x => x.User.ProfileId).NotEmpty().WithMessage("El campo ProfileId es obligatorio");
        RuleFor(x => x.User.RoleIds)
            .NotEmpty().WithMessage("La propiedad RoleIds no debe estar vacía")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Todos los Ids debe ser enteros positivos");
        RuleFor(x => x.User.ProviderIds)
            .NotNull()
            .WithMessage("La propiedad ProviderIds no debe ser null")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Todos los Ids de ProviderIds debe ser enteros positivos");
        RuleFor(x => x.User.ClientIdS)
            .NotNull()
            .WithMessage("La propiedad ClientIdS no debe ser null")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Todos los Ids de ClientIdS debe ser enteros positivos");
    }
}
