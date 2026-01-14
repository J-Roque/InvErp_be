using BuildingBlocks.CQRS;
using FluentValidation;
using Security.Application.Dtos.General;

namespace Security.Application.Handlers.Profiles.Commands.CreateProfile;

public record CreateProfileCommand(ProfileDto Profile, UserContextDto UserContext) : ICommand<CreateProfileResult>;

public record CreateProfileResult(long Id);

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Profile).NotEmpty().WithMessage("El objeto Profile es obligatorio");
        RuleFor(x => x.Profile.Name).NotEmpty().WithMessage("El campo Name es obligatorio");
        RuleFor(x => x.Profile.IsActive)
            .NotNull().WithMessage("El campo IsActive es obligatorio")
            .Must(value => value == true || value == false).WithMessage("El campo IsActive debe ser un booleano");
        RuleFor(x => x.Profile.NavigationItemIds)
            .NotNull().WithMessage("La propiedad NavigationItemIds no debe ser nula");
    }
}
