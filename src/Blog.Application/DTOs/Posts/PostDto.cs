using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;

namespace Blog.Application.DTOs.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public UserDto Author { get; set; }
}
