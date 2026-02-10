using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;

namespace Blog.Domain.Entities;

public class User
{
    private int maxBioLength = 1000;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public Email Email { get; set; }
    public string Bio { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<PostLike> LikedPosts { get; set; } = new List<PostLike>();
    public List<Post> Posts { get; set; } = new List<Post>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    protected User() { }
    public User(string _name, Email _email, string _bio)
    {
        Name = _name;
        Email = _email;
        Bio = _bio;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ValueCannotBeEmptyException("Name");

        if (Name == newName) return;

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBio(string newBio)
    {
        if (string.IsNullOrWhiteSpace(newBio))
            throw new ValueCannotBeEmptyException("Bio");

        if (newBio.Length > maxBioLength)
            throw new ExceedCaractersNumberException("Bio");

        if (Bio == newBio) return;

        Bio = newBio;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

}
