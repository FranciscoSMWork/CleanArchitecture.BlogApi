using Blog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities;

public class Post
{
    private int maxContentLength = 1000;

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } = null!;
    public User Author { get; set; }
    public Guid AuthorId; 

    public List<PostTag> Tags { get; set; } = new List<PostTag>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<PostLike> Likes { get; set; } = new List<PostLike>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    protected Post() { }
    public Post(string _title, string _content, User _author)
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new ValueCannotBeEmptyException("Title");

        if (_author == null)
            throw new ValueCannotBeEmptyException("Author");

        this.Title = _title;
        this.Author = _author;
        this.AuthorId = _author.Id;

        UpdateContent(_content);
    }

    public void UpdateContent(string newContent)
    {
        if (newContent.Length > maxContentLength)
            throw new ExceedCaractersNumberException("Content");

        this.Content = newContent ?? "";
    }



}
