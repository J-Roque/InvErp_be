using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

public class AuthLogConfiguration: IEntityTypeConfiguration<AuthLog>
{
    public void Configure(EntityTypeBuilder<AuthLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Content)
            .HasColumnType("nvarchar(max)");

        builder.ToTable("AuthLogs");
    }
}
