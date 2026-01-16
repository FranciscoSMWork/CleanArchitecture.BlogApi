using Blog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Post Post { get; set; }
    public Guid PostId { get; set; }
    public User Author { get; set; }
    public Guid AuthorId;
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    protected Comment() { }
    public Comment(Post post, User author, string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ValueCannotBeEmptyException("Content");

        if (author == null)
            throw new ValueCannotBeEmptyException("Author");

        if (post == null)
            throw new ValueCannotBeEmptyException("Post");

        this.Post = post;
        this.Author = author;
        this.AuthorId = author.Id;
        this.Content = content;
    }

    public void UpdateComment(string newContent)
    {
        if (string.IsNullOrEmpty(newContent))
            throw new ValueCannotBeEmptyException("Content");
    }
}
