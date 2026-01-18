using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(u => u.Name)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion
            (
                new EmailConverter()
            )
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Bio)
            .HasMaxLength(1000);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt);
        builder.Property(u => u.DeletedAt)
            .IsRequired(false);
    }
}
