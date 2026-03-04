using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class Post
{
    private int maxContentLength = 1000;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Content { get; private set; } = null!;
    public User Author { get; private set; }
    public Guid AuthorId; 

    public List<PostTag> Tags { get; private set; } = new List<PostTag>();
    public List<Comment> Comments { get; private set; } = new List<Comment>();
    public List<PostLike> Likes { get; private set; } = new List<PostLike>();

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }

    protected Post() { }
    public Post(string title, string content, User author)
    {
        SetTitle(title);
        SetAuthor(author);
        SetContent(content);
    }

    private void SetAuthor(User author)
    {
        if (author == null)
            throw new ValueCannotBeEmptyException("Author");

        this.Author = author;
        this.AuthorId = author.Id;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValueCannotBeEmptyException("Title");

        this.Title = title;
    }

    private void SetContent(string content)
    {
       if (string.IsNullOrWhiteSpace(content))
            throw new ValueCannotBeEmptyException("Post");

       if (content.Length > maxContentLength)
            throw new ExceedCaractersNumberException("Post");

        this.Content = content;
    }

    public void UpdateContent(string newContent)
    {
        SetContent(newContent);
        this.UpdatedAt = DateTime.Now;
    }
    public void UpdateTitle(string newTitle)
    {
        SetTitle(newTitle);
        this.UpdatedAt = DateTime.Now;
    }
}
