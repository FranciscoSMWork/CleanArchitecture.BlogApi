using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Configurations;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {

        builder.ToTable("PostLikes");

        builder.HasKey(pl => pl.Id);
        builder.Property(pl => pl.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(pl => pl.PostId)
            .IsRequired();

        builder.Property(pl => pl.UserId)
            .IsRequired();

        builder.HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pl => pl.User)
            .WithMany(u => u.LikedPosts)
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pl => pl.PostId);
        builder.HasIndex(pl => pl.UserId);

        builder.HasIndex(p => new { p.PostId, p.UserId }).IsUnique();
    }
}
