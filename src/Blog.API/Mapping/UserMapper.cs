using Blog.API.Dtos.Users;
using Blog.Application.DTOs.Users;

namespace Blog.API.Mapping;

public static class UserMapper
{
    public static CreateUserDto toCreateDto( this CreateUserRequest request)
    {
        return new CreateUserDto
        {
            Name = request.Name,
            Email = request.Email,
            Bio = request.Bio
        };
    }

    public static UpdateUserDto ToUpdateDto( this UpdateUserRequest request)
    {
        return new UpdateUserDto
        {
            Name = request.Name,
            Email = request.Email,
            Bio = request.Bio
        };
    }

    public static UserResponse ToResponse(this UserDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Bio = dto.Bio
        };
    }
}
