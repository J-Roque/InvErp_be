using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Domain.Enums;

namespace Security.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255);

            builder.Property(x => x.Code)
                .HasMaxLength(255);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.IsProtected)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IdentityStatusId)
            .HasDefaultValue(IdentityStatus.Active)
            .HasConversion(
                u => (int)u,
                dbStatus => (IdentityStatus)dbStatus
            );
        }
    }
}
