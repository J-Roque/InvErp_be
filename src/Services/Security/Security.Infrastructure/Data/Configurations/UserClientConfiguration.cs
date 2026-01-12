using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations
{
    public class UserClientConfiguration : IEntityTypeConfiguration<UserClient>
    {
        public void Configure(EntityTypeBuilder<UserClient> builder)
        {
            builder.HasKey(uc => uc.Id);
            builder.Property(uc => uc.Id).ValueGeneratedOnAdd();
            
            builder.Property(uc => uc.UserId)
                .IsRequired();
            
            builder.Property(uc => uc.ClientId)
                .IsRequired();
            
            builder.Property(uc => uc.ClientPersonId)
                .IsRequired();
            
            builder.Property(uc => uc.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
            
            // Relación con User
            builder.HasOne<User>()
                .WithMany(u => u.UserClients)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Índice para búsquedas rápidas
            builder.HasIndex(uc => new { uc.UserId, uc.ClientId });
        }
    }
}
