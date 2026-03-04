using Blog.API.Dtos.Users;
using Blog.Application.DTOs.Users;

namespace Blog.API.Dtos.Posts;

public class PostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }

}
