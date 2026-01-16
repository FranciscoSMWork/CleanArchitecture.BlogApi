using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class PostLike
{
    public int Id { get; set; }
    public User User { get; set; }
    public Guid UserId;
    public Post Post { get; set; }
    public Guid PostId;

    protected PostLike() { }
    public PostLike(User _user, Post _post)
    {
        if (_user == null)
            throw new ValueCannotBeEmptyException("Author");

        if (_post == null)
            throw new ValueCannotBeEmptyException("Post");

        this.User = _user;
        this.Post = _post;
    }
}
