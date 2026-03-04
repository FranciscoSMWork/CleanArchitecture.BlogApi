using Blog.API.Dtos.Users;
using Blog.API.Mapping;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = request.toCreateDto();
        UserDto userDto = await _userService.AddUser(user);
        UserResponse userResponse = userDto.ToResponse();
        return Created($"/api/users/{userDto.Id}", userResponse);
    }

    //GET /posts retorna lista
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var users = await _userService.ListAllUsers();
        var response = users.Select(user => user.ToResponse()).ToList();
        return Ok(response);
    }

    //GET /users/{id} retorna usuário existente
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var userDto = await _userService.FindUserById(Id);
        var response = userDto.ToResponse();
        return Ok(response);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(Guid Id, [FromBody] UpdateUserRequest request)
    {
        UpdateUserDto updateUserDto = request.ToUpdateDto();
        var updated = await _userService.Update(Id, updateUserDto);
        return Ok(updated);
    }
}
