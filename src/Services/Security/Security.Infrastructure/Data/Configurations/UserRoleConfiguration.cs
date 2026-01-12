using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            
            builder.Property(ur => ur.UserId)
                .IsRequired();
            
            builder.Property(ur => ur.RoleId)
                .IsRequired();
            
            // Relación con User
            builder.HasOne<User>()
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
