using Blog.API.Dtos.Posts;
using Blog.API.Dtos.Users;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Posts;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        this._postService = postService;
    }

    [HttpPost]
    [Route("api/posts")]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var postDto = new CreatePostDto
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId
        };

        PostResultDto postResultDto = await _postService.AddAsync(postDto);

        PostResponse postResponse = new PostResponse
        {
            Id = postResultDto.Id,
            Title = postResultDto.Title,
            Content = postResultDto.Content,
            AuthorId = postResultDto.AuthorId,
            Author = new UserResponse
            {
                Id = postResultDto.Author.Id,
                Name = postResultDto.Author.Name,
                Email = postResultDto.Author.Email.Address,
                Bio = postResultDto.Author.Bio
            }
        };

        return Created($"/posts/{postResultDto.Id}", postResponse);
    }

    [HttpGet]
    [Route("/api/posts")]
    public async Task<IActionResult> List()
    {
        var posts = await _postService.ListAllPostAsync();

        var response = posts.Select(post => new PostResponse
        {
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.Author.Id
        }).ToList();

        return Ok(response);
    }

    [HttpGet]
    [Route("/api/posts/{id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var post = await _postService.FindPostById(Id);
        if (post == null)
            return NotFound();

        var response = new PostResponse
        {
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.Author.Id,
            Author = new UserResponse
            {
                Id = post.Author.Id,
                Name = post.Author.Name,
                Email = post.Author.Email.Address,
                Bio = post.Author.Bio
            }
        };

        return Ok(response);
    }

    [HttpPut]
    [Route("/api/posts/{id}")]
    public async Task<IActionResult> Update(Guid Id, [FromBody] UpdatePostRequest updatePostRequest)
    {
        var post = await _postService.FindPostById(Id);
        if (post == null)
            return NotFound();

        var response = new PostResponse
        {
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.Author.Id,
            Author = new UserResponse
            {
                Id = post.Author.Id,
                Name = post.Author.Name,
                Email = post.Author.Email.Address,
                Bio = post.Author.Bio
            }
        };

        return Ok(response);
    }

    [HttpDelete]
    [Route("/api/posts/{id}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var post = await _postService.FindPostById(Id);
        if (post == null)
            return NotFound();

        var response = await _postService.DeletePostAsync(Id);

        return Ok(response);
    }
}
