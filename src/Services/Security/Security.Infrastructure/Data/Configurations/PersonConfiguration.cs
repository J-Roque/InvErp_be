using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Infrastructure.Data.Configurations;

public class PersonConfiguration: IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    { 
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.FirstName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(255).IsRequired();
        
        builder.Property(x => x.DocumentTypeId)
            .IsRequired();
        
        builder.Property(x => x.DocumentNumber)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasMaxLength(255);
        
        builder.HasMany(x => x.Contacts)
            .WithOne()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ComplexProperty(
            x => x.PersonType, personType =>
            {
                personType.Property(u => u.Value)
                    .HasColumnName(nameof(Person.PersonType))
                    .HasMaxLength(20)
                    .IsRequired();
            }
        );
        
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.ToTable("Persons");
        
    }
}