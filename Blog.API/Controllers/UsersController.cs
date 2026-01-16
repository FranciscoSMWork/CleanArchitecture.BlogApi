using Blog.API.Dtos.Users;
using Blog.Application.Commands.Users;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


/*    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.FindUserById(id);

        if (user is null)
            return NotFound();

        var response = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email.ToString(),
            Bio = user.Bio
        };

        return Ok(response);
    }*/
}
