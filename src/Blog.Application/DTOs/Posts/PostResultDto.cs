using Blog.Domain.Entities;

namespace Blog.Application.DTOs.Posts;

public class PostResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
}
