using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Comments;
using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.ValueObjects;

namespace Blog.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    
    public UserService(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork
        )
    {
        this._userRepository = userRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<UserDto> FindUserById(Guid Id)
    {
        var user = await _userRepository.GetByIdAsync(Id);

        if (user == null || user.DeletedAt != null)
            throw new NotFoundException("User");

        UserDto userDto = new UserDto
        {
            Id = Id,
            Name = user.Name,
            Email = user.Email.Address,
            Bio = user.Bio
        };

        return userDto;
    }

    public async Task<UserDto> AddUser(CreateUserDto dto)
    {
        Email email = new Email(dto.Email);
        if (await _userRepository.EmailExists(email))
        {
            throw new AlreadyExistsException("Email");
        }
        User user = new User(dto.Name, email, dto.Bio);
        User createdUser = await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        UserDto userDto = new UserDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email.Address,
            Bio = createdUser.Bio
        };
        return userDto;
    }

    public async Task<List<UserDto>> ListAllUsers()
    {
        List<User> listUser = await _userRepository.ListAllAsync();
        List<UserDto> listUserDto = listUser
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Address,
                Bio = user.Bio
            })
            .ToList();

        return listUserDto;
    }

    public async Task<UserDto> Update(Guid Id, UpdateUserDto dto)
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

        UserDto userDto = new UserDto
        {
           Id = user.Id,
           Name = user.Name,
           Email = user.Email.Address,
           Bio = user.Bio
        };

        return userDto;
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
