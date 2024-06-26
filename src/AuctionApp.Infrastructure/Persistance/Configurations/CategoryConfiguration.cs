﻿using EntityFramework.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        builder
            .Property(e => e.Name)
            .HasMaxLength(256)
            .IsRequired();
    }
}
