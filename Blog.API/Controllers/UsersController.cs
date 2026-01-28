using Blog.API.Dtos.Users;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("api/users")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = new CreateUserDto
        {
            Name = request.Name,
            Email = request.Email,
            Bio = request.Bio
        };

        await _userService.AddUser(user);

        return Created($"/users/{user.Name}", user);
    }

    //GET /posts retorna lista
    [HttpGet]
    [Route("api/users")]
    public async Task<IActionResult> List()
    {
        var users = await _userService.ListAllUsers();

        var response = users.Select(user => new UserResponse
        {
            Name = user.Name,
            Email = user.Email.Address,
            Bio = user.Bio
        });

        return Ok(response);
    }

    //GET /users/{id} retorna usuário existente
    [HttpGet]
    [Route("api/users/{id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var user = await _userService.FindUserById(Id);
        if (user == null)
                return NotFound();

        var response = new GetUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email.Address,
            Bio = user.Bio
        };

        return Ok(response);
    }
}
