using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations;

// MenuItem - Profile
public class ProfileMenuItemConfiguration: IEntityTypeConfiguration<ProfileNavigationItem>
{
    public void Configure(EntityTypeBuilder<ProfileNavigationItem> builder)
    {
        builder.HasKey(x => new { x.ProfileId, x.NavigationItemId });
        
        builder
            .HasOne<Profile>()
            .WithMany(p => p.ProfileNavigationItems)
            .HasForeignKey(pm => pm.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<NavigationItem>()
            .WithMany()
            .HasForeignKey(pm => pm.NavigationItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}