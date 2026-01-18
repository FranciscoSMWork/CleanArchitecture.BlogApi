using Blog.Application.Abstractions.Services;
using Blog.Application.Commands.Users;
using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        this._userRepository = userRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<User> FindUserById(Guid Id)
    {
        return await _userRepository.GetByIdAsync(Id);
    }

    public async Task<bool> AddUser(CreateUserDto dto)
    {
        Email email = new Email(dto.Email);
        User user = new User(dto.Name, email, dto.Bio);
        return await _userRepository.AddAsync(user);
    }

    public async Task<List<User>> ListAllUsers()
    {
        return await _userRepository.ListAllAsync();
    }

    public async Task<bool> Update(User user)
    {
        return await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }

}
