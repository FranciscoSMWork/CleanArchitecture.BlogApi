using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;

namespace Blog.Domain.Entities;

public class User
{
    private int maxBioLength = 1000;

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Email Email { get; set; }
    public string Bio { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<PostLike> LikedPosts { get; set; } = new List<PostLike>();
    public List<Post> Posts { get; set; } = new List<Post>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    protected User() { }
    public User(string _name, Email _email, string _bio)
    {
        Name = _name;
        Email = _email;
        Bio = _bio;
    }

    public void UpdateBio(string newBio)
    {
        if (newBio.Length > maxBioLength)
            throw new ExceedCaractersNumberException("Bio");

        Bio = newBio ?? "";
    }
}
