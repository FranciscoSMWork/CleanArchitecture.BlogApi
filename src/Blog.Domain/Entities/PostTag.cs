using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;
public class PostTag
{
    public int Id { get; private set; }
    public Post Post { get; private set; } = null!;
    public Guid PostId;
    public Tag Tag { get; private set; } = null!;
    public int TagId;
    protected PostTag() { }
    public PostTag(Post post, Tag tag)
    {
        SetPost(post);
        SetTag(tag);
    }

    private void SetPost(Post post)
    {
        if (post == null)
            throw new ValueCannotBeEmptyException("Post");

        this.Post = post;
    }

    private void SetTag(Tag tag)
    { 
        if (tag == null)
            throw new ValueCannotBeEmptyException("Tag");

        this.Tag = tag;
    }
}
