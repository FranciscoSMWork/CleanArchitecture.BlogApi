using Azure;
using Blog.API.Dtos.Comments;
using Blog.API.Mapping;
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
        var commentCreateDto = request.toCreateDto();
        CommentDto commentDto = await _commentService.AddCommentAsync(commentCreateDto);
        CommentResponse commentResponse = commentDto.toResponse();
        return Created($"/posts/{commentDto.Id}", commentResponse);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        List<CommentResponse> listComment = new List<CommentResponse>();
        var comments = await _commentService.ListCommentAsync();
        var response = comments.Select(comment => comment.toResponse()).ToList();
        return Ok(response);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var comment = await _commentService.FindCommentById(id);
        var response = await _commentService.DeleteCommentAsync(id);
        return Ok(response);
    }
}
