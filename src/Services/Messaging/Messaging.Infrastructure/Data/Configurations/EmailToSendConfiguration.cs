using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messaging.Infrastructure.Data.Configurations;

public class EmailToSendConfiguration : IEntityTypeConfiguration<EmailToSend>
{
    public void Configure(EntityTypeBuilder<EmailToSend> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Subject)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Template)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Data)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.Recipients)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.Cc)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Bcc)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.IsSent)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.SentDate);

        builder.Property(x => x.Priority)
            .HasDefaultValue(1)
            .IsRequired();

        builder.ToTable("EmailsToSend");
    }
}