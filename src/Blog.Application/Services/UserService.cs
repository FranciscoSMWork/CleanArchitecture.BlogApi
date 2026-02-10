using Blog.Application.Abstractions.Services;
using Blog.Application.Commands.Users;
using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
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

    public async Task<UserResultDto> AddUser(CreateUserDto dto)
    {
        Email email = new Email(dto.Email);
        if (await _userRepository.EmailExists(email))
        {
            throw new AlreadyExistsException("Email");
        }
        User user = new User(dto.Name, email, dto.Bio);
        User createdUser = await _userRepository.AddAsync(user);

        UserResultDto userResultDto = new UserResultDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email.Address,
            Bio = createdUser.Bio
        };
        return userResultDto;
    }

    public async Task<List<User>> ListAllUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> Update(Guid Id, UpdateUserDto dto)
    {
        User user = await _userRepository.GetByIdAsync(Id);

        if (user == null || user.DeletedAt != null)
            throw new NotFoundException("User");

        if (dto.Name != null)
            user.UpdateName(dto.Name);

        if (dto.Email != null)
            user.UpdateEmail(new Email(dto.Email));

        if (dto.Bio != null)
            user.UpdateBio(dto.Bio);

        await _unitOfWork.CommitAsync();

        return user;
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    public async Task<bool> EmailExistsAsync(Email email)
    {
       return await _userRepository.EmailExists(email);
    }
}
