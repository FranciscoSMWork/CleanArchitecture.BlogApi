using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Abstractions.Services;

public interface IUserService
{
    Task<UserDto> AddUser(CreateUserDto dto);
    Task<UserDto?> FindUserById(Guid id);
    Task<List<UserDto>> ListAllUsers();
    Task<UserDto> Update(Guid Id, UpdateUserDto user);
    Task<bool> Delete(Guid id);
    Task<bool> EmailExistsAsync(Email email);
}