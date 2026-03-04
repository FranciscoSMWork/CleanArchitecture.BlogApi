using Blog.API.Dtos.Comments;
using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.Application.DTOs.Comments;

namespace Blog.API.Mapping;

public static class CommentMapper
{
    public static CommentCreateDto toCreateDto (this CreateCommentRequest request)
    {
        return new CommentCreateDto
        {   
            PostId = request.PostId,
            AuthorId = request.AuthorId,
            Content = request.Content
        }; 
    }

    public static CommentResponse toResponse (this CommentDto commentDto)
    {
        return new CommentResponse
        {
            Id = commentDto.Id,
            PostId = commentDto.PostId,
            Post = commentDto.Post,
            AuthorId = commentDto.AuthorId,
            Author = commentDto.Author,
            Content = commentDto.Content,
        };
    }
}
