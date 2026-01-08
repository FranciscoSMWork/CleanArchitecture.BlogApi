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

    public int Id;
    public string Title { get; set; }
    public string Content { get; set; }
    public User Author { get; set; } 

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Post(string _title, string _content, User _author)
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new ValueCannotBeEmptyException("Title");

        if (_author == null)
            throw new ValueCannotBeEmptyException("Author");

        Title = _title;
        Content = _content;

        Author = _author;
    }

    public void UpdateContent(string newContent)
    {
        if (newContent.Length > maxContentLength)
            throw new ExceedCaractersNumberException("Content");

        Content = newContent ?? "";
    }

}
