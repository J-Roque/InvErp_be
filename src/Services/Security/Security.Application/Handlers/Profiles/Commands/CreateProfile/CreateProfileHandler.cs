using BuildingBlocks.CQRS;
using Security.Application.Data;
using Security.Application.Dtos.General;
using Security.Domain.Models;

namespace Security.Application.Handlers.Profiles.Commands.CreateProfile;

public class CreateProfileHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateProfileCommand, CreateProfileResult>
{
    public async Task<CreateProfileResult> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = CreateNewProfile(command.Profile);

        // Se guardan los navigation items
        if (command.Profile.NavigationItemIds.Count > 0)
        {
            foreach (var navigationItemId in command.Profile.NavigationItemIds)
            {
                profile.SetNavigationItem(navigationItemId);
            }
        }

        dbContext.Profiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProfileResult(profile.Id);
    }

    private Profile CreateNewProfile(ProfileDto profileDto)
    {
        var newProfile = Profile.Create(
            id: 0,
            name: profileDto.Name,
            isActive: profileDto.IsActive,
            imageAttachmentId: null,
            code: null,
            isProtected: false
        );

        return newProfile;
    }
}
