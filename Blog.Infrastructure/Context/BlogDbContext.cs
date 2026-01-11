using Blog.Domain.Entities;
using Blog.Infrastructure.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Context;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>()
            .HasQueryFilter(p => p.DeletedAt == null);

        modelBuilder.Entity<Comment>()
            .HasQueryFilter(c => c.DeletedAt == null);

        modelBuilder.Entity<PostLike>();

        modelBuilder.Entity<PostTag>();

        modelBuilder.Entity<Tag>()
            .HasQueryFilter(t => t.DeletedAt == null);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasConversion(new EmailConverter());

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Bio)
                .HasMaxLength(500);
        });
    }
}
