using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.Application.DTOs.Posts;
using Blog.Application.DTOs.Users;

namespace Blog.API.Mapping;

public static class PostMapper
{
    public static CreatePostDto toCreatePost(this CreatePostRequest request)
    {
        return new CreatePostDto
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId
        };
    }

    public static UpdatePostDto toUpdatePost(this UpdatePostRequest request)
    {
        return new UpdatePostDto
        {
            Title = request.Title,
            Content = request.Content
        };
    }

    public static PostResponse toResponse(this PostDto postDto)
    {
        UserResponse userDto = new UserResponse
        {
            Id = postDto.Author.Id,
            Name = postDto.Author.Name,
            Email = postDto.Author.Email,
            Bio = postDto.Author.Bio
        };

        return new PostResponse
        {
            Id = postDto.Id,
            Title = postDto.Title,
            Content = postDto.Content,
            AuthorId = postDto.AuthorId,
            Author = userDto
        };
    }
}
