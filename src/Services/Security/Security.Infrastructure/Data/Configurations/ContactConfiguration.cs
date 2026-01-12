using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Security.Infrastructure.Data.Configurations;

public class ContactConfiguration: IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(255).IsRequired();

        builder.Property(x => x.Position)
            .HasMaxLength(255);

        builder.Property(c => c.PhoneNumber)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default),
                new ValueComparer<List<string>?>(
                    (c1, c2) => JsonSerializer.Serialize(c1, JsonSerializerOptions.Default) == JsonSerializer.Serialize(c2, JsonSerializerOptions.Default), 
                    c => c == null ? 0 : JsonSerializer.Serialize(c, JsonSerializerOptions.Default).GetHashCode(),
                    c => c == null ? null : JsonSerializer.Deserialize<List<string>>(JsonSerializer.Serialize(c, JsonSerializerOptions.Default), JsonSerializerOptions.Default))
            )
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Mobile)
            .HasMaxLength(20);
    }
}