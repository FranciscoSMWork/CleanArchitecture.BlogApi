namespace Blog.Domain.Entities;
public class PostTag
{
    public int Id { get; set; }
    public Post Post { get; set; }
    public Guid PostId;
    public Tag Tag { get; set; }
    public int TagId;
    protected PostTag() { }
    public PostTag(Post post, Tag tag)
    {
        this.Post = post;
        this.Tag = tag;
    }
}
