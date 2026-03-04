using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.Application.DTOs.Posts;
using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;

namespace Blog.API.Dtos.Comments;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public PostDto Post{get; set;}
    public Guid AuthorId { get; set; }
    public UserDto Author { get; set; }
    public string Content { get; set; }
}
