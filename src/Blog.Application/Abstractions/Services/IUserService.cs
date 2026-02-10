using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Abstractions.Services;

public interface IUserService
{
    Task<UserResultDto> AddUser(CreateUserDto dto);
    Task<User?> FindUserById(Guid id);
    Task<List<User>> ListAllUsers();
    Task<User> Update(Guid Id, UpdateUserDto user);
    Task<bool> Delete(Guid id);
    Task<bool> EmailExistsAsync(Email email);
}