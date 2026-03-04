using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PostId { get; private set; }
    public Post Post;
    public User Author { get; private set; }
    public Guid AuthorId;
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }

    protected Comment() { }
    public Comment(Post post, User author, string content)
    {
        SetPost(post);
        SetAuthor(author);
        SetContent(content);
    }

    private void SetPost(Post post)
    {
        if (post == null)
            throw new ValueCannotBeEmptyException("Post");

        this.Post = post;
        this.PostId = post.Id;
    }
    
    private void SetAuthor(User author)
    {
        if (author == null)
            throw new ValueCannotBeEmptyException("Author");

        this.Author = author;
        this.AuthorId = author.Id;
    }

    private void SetContent(string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ValueCannotBeEmptyException("Content");
     
        this.Content = content;
    }

    public void UpdatePost(Post post)
    {
        SetPost(post);
        this.UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateAuthor(User author)
    {
        SetAuthor(author);
        this.UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateContent(string content)
    {
        SetContent(content);
        this.UpdatedAt = DateTime.UtcNow;
    }
}
