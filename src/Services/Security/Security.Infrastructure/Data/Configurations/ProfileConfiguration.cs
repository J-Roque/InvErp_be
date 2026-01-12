using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations;

public class ProfileConfiguration: IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(255);
            
        builder.HasIndex(x => x.Code)
            .IsUnique();
        
        builder.Property(x => x.IsProtected)
            .HasDefaultValue(false)
            .IsRequired();
        
    }
}