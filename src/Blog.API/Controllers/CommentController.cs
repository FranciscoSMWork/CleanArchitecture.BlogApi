using Azure;
using Blog.API.Dtos.Comments;
using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Comments;
using Blog.Application.DTOs.Posts;
using Blog.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
    {
        var commentCreateDto = new CommentCreateDto
        {
            Content = request.Content,
            PostId = request.PostId,
            AuthorId = request.AuthorId
        };

        CommentDto commentDto = await _commentService.AddCommentAsync(commentCreateDto);

        CommentResponse commentResponse = new CommentResponse
        {
            Id = commentDto.Id,
            Content = commentDto.Content,
            PostId = commentDto.PostId,
            Post = new PostResponse
            {
                Id = commentDto.Post.Id,
                Title = commentDto.Post.Title,
                Content = commentDto.Post.Content,
                AuthorId = commentDto.Post.Author.Id,
                Author = new UserResponse
                {
                    Id = commentDto.Post.Author.Id,
                    Name = commentDto.Post.Author.Name,
                    Email = commentDto.Post.Author.Email.Address,
                    Bio = commentDto.Post.Author.Bio
                }
                    
            },
            AuthorId = commentDto.AuthorId,
            Author = new UserResponse
            {
                Id = commentDto.Author.Id,
                Name = commentDto.Author.Name,
                Email = commentDto.Author.Email.Address,
                Bio = commentDto.Author.Bio
            }
        };

        return Created($"/posts/{commentDto.Id}", commentResponse);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var comments = await _commentService.ListCommentAsync();

        List<CommentResponse> response = comments.Select(comment => new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            PostId = comment.PostId,
            Post = new PostResponse
            {
                Id = comment.Post.Id,
                Title = comment.Post.Title,
                Content = comment.Post.Content,
                AuthorId = comment.Post.Author.Id,
                Author = new UserResponse
                {
                    Id = comment.Post.Author.Id,
                    Name = comment.Post.Author.Name,
                    Email = comment.Post.Author.Email.Address,
                    Bio = comment.Post.Author.Bio
                }
            },
            AuthorId = comment.AuthorId,
            Author = new UserResponse
            {
                Id = comment.Author.Id,
                Name = comment.Author.Name,
                Email = comment.Author.Email.Address,
                Bio = comment.Author.Bio
            }
        }).ToList();

        return Ok(response);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var comment = await _commentService.FindCommentById(id);
        if (comment == null)
            return NotFound();

        var response = await _commentService.DeleteCommentAsync(id);
        return Ok(response);
    }
}
