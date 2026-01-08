using Blog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public Post Post { get; set; }
    public User Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public Comment(Post post, User author, string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ValueCannotBeEmptyException("Content");

        if (author == null)
            throw new ValueCannotBeEmptyException("Author");

        if (post == null)
            throw new ValueCannotBeEmptyException("Post");

        Post = post;
        Author = author;        
        Content = content;
    }

    public void UpdateComment(string newContent)
    {
        if (string.IsNullOrEmpty(newContent))
            throw new ValueCannotBeEmptyException("Content");
    }
}
