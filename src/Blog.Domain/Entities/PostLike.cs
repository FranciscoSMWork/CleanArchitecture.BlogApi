using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using System.Xml.Linq;

namespace Blog.Domain.Entities;

public class PostLike
{
    public int Id { get; private set; }
    public User User { get; private set; } = null!;
    public Guid UserId;
    public Post Post { get; private set; } = null!;
    public Guid PostId;

    protected PostLike() { }
    public PostLike(User user, Post post)
    {
        SetUser(user);
        SetPost(post);   
    }

    private void SetUser(User user)
    {
        if (user == null)
            throw new ValueCannotBeEmptyException("User");

        this.User = user;
    }

    private void SetPost(Post post)
    {
        if (post == null)
            throw new ValueCannotBeEmptyException("Post");

        this.Post = post;
    }
}
