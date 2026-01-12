using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations
{
    public class UserProviderConfiguration : IEntityTypeConfiguration<UserProvider>
    {
        public void Configure(EntityTypeBuilder<UserProvider> builder)
        {
            builder.HasKey(up => up.Id);
            builder.Property(up => up.Id).ValueGeneratedOnAdd();
            
            builder.Property(up => up.UserId)
                .IsRequired();
            
            builder.Property(up => up.ProviderId)
                .IsRequired();
            
            builder.Property(up => up.ProviderPersonId)
                .IsRequired();
            
            builder.Property(up => up.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
            
            // Relación con User
            builder.HasOne<User>()
                .WithMany(u => u.UserProviders)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Índice para búsquedas rápidas
            builder.HasIndex(up => new { up.UserId, up.ProviderId });
        }
    }
}
