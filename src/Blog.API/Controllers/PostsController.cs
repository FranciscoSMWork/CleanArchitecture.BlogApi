using Azure.Core;
using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.API.Mapping;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Posts;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        this._postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var postCreateDto = request.toCreatePost();
        PostDto postDto = await _postService.AddAsync(postCreateDto);
        PostResponse postResponse = postDto.toResponse();
        return CreatedAtAction(nameof(Get), new { Id = postResponse.Id }, postResponse);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var posts = await _postService.ListAllPostAsync();
        var response = posts.Select(post => post.toResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var post = await _postService.FindPostById(Id);
        var response = post.toResponse();
        return Ok(response);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(Guid Id, [FromBody] UpdatePostRequest request)
    {
        UpdatePostDto updatePostDto = request.toUpdatePost();
        var post = await _postService.UpdatePostAsync(Id, updatePostDto);
        var response = post.toResponse();
        return Ok(response);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var post = await _postService.FindPostById(Id);
        var response = await _postService.DeletePostAsync(Id);
        return Ok(response);
    }
}
