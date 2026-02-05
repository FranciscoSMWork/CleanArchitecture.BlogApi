using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
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
