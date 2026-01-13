using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.GeneratedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.LastUsedAt)
            .IsRequired();

        builder.Property(x => x.Ip)
            .HasMaxLength(50);

        builder.Property(x => x.Device)
            .HasMaxLength(255);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(255);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.ToTable("Tokens");
    }
}
