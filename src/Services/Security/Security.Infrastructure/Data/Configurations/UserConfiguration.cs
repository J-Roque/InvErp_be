using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Domain.Enums;

namespace Security.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            
            builder.Property(u => u.Username)
                .HasMaxLength(255)
                .IsRequired();
            
            builder.HasIndex(u => u.Username)
                .IsUnique();
            
            builder.Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();
            
            builder.Property(u => u.Salt)
                .HasMaxLength(255)
                .IsRequired();
            
            builder.Property(u => u.IdentityStatusId)
            .HasDefaultValue(IdentityStatus.Active)
            .HasSentinel((IdentityStatus)0)
            .HasConversion(
                u => (int) u,
                dbStatus => (IdentityStatus) dbStatus
            );

            builder.Property(x => x.PersonId)
                .IsRequired();
            
            builder.HasOne<Person>()
                .WithOne()
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
            
            
            builder.HasOne<Profile>()
                .WithMany()
                .HasForeignKey(u => u.ProfileId);
            
            // Configuración de colecciones de navegación usando backing fields
            builder.Navigation(u => u.UserRoles)
                .HasField("_userRoles")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Navigation(u => u.UserProviders)
                .HasField("_userProviders")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Navigation(u => u.UserClients)
                .HasField("_userClients")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
                
            // Si es un ValueObject el Id (guid)
            //builder.Property(c => c.Id).HasConversion(
            //     userId => userId.Value,
            //     dbId => dbId => UserId.Of(dbId));
        }
    }
}
