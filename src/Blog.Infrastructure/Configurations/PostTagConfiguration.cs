using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Configurations;

public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {

        builder.ToTable("PostTags");

        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(pt => pt.PostId)
            .IsRequired();

        builder.Property(pt => pt.TagId)
            .IsRequired();

        builder.HasIndex(pt => pt.PostId);
        builder.HasIndex(pt => pt.TagId);

        builder.HasIndex(pt => new { pt.PostId, pt.TagId }).IsUnique();
    }
}
