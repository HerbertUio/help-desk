using Domain.Dtos;
using Domain.Models;

namespace Application.Extensions;

public static class UserExtend
{
    public static UserModel ToModel (this RegisterUserDto registerUserDto)
    {
        return new UserModel(
            0,
            registerUserDto.Name,
            registerUserDto.LastName,
            registerUserDto.PhoneNumber,
            registerUserDto.Email,
            registerUserDto.Password,
            registerUserDto.DepartmentId,
            registerUserDto.Role,
            true
        );
    }
    
}