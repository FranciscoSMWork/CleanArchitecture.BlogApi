using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;

namespace Blog.Domain.Entities;

public class User
{
    private const int maxBioLength = 1000;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Bio { get; private set; }

    private readonly List<Comment> _comments = new();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    
    private readonly List<PostLike> _likedPosts = new();
    public IReadOnlyCollection<PostLike> LikedPosts => _likedPosts.AsReadOnly();

    public readonly List<Post> _posts = new();
    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }

    protected User() { }
    public User(string name, Email email, string bio)
    {
        SetName(name);
        SetEmail(email);
        SetBio(bio);
    }

    private void SetName(string name) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValueCannotBeEmptyException("Name");

        this.Name = name;
    }

    private void SetEmail(Email email)
    {
        if (string.IsNullOrWhiteSpace(email.Address))
            throw new ValueCannotBeEmptyException("Email");

        this.Email = email;
    }

    private void SetBio(string bio)
    {
        if (string.IsNullOrWhiteSpace(bio))
            throw new ValueCannotBeEmptyException("Bio");

        if (bio.Length > maxBioLength)
            throw new ExceedCaractersNumberException("Bio");

        this.Bio = bio;
    }

    public void UpdateName(string newName)
    {
        SetName(newName);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBio(string newBio)
    {
        SetBio(newBio);
        this.UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email newEmail)
    {
        SetEmail(newEmail);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
