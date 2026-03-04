using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class PostLike
{
    public int Id { get; private set; }
    public User User { get; private set; }
    public Guid UserId;
    public Post Post { get; private set; }
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
