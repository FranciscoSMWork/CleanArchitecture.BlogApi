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
}
