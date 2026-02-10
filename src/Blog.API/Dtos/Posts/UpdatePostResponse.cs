using Blog.API.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.API.Dtos.Posts;

public class UpdatePostResponse
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;
    public UserResponse Author { get; set; }
    public Guid AuthorId;

}
