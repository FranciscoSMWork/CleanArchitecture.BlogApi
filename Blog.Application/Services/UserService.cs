using Blog.Application.Commands.Users;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<bool> AddUser(User user)
    {
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

    public async Task<User> CreateUser(CreateUserCommand command)
    {
        var email = new Email(command.Email);

        if (await _userRepository.EmailExists(email))
            throw new InvalidOperationException("Email already exists");

        var user = new User(
            command.Name,
            email,
            command.Bio
        );

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return user;
    }
}
