using Domain.Dtos;
using Domain.Models;

namespace Application.Extensions;

public static class UserExtend
{
    
    // Mapea RegisterUserDto -> UserModel (para creación)
    public static UserModel ToModel (this RegisterUserDto registerUserDto)
    {
        return new UserModel(
            0, // ID se genera en la DB
            registerUserDto.Name,
            registerUserDto.LastName,
            registerUserDto.PhoneNumber,
            registerUserDto.Email,
            "", // Password se hashea y asigna en el servicio
            registerUserDto.DepartmentId,
            registerUserDto.Role,
            true // Usuario activo por defecto al registrar
        );
    }

    // Mapea UserModel -> UserDto (para devolver listas/detalles SIN password)
    public static UserDto ToUserDto (this UserModel userModel)
    {
        return new UserDto
        {
            Id = userModel.Id,
            Name = userModel.Name,
            LastName = userModel.LastName,
            PhoneNumber = userModel.PhoneNumber,
            Email = userModel.Email,
            DepartmentId = userModel.DepartmentId,
            Role = userModel.Role,
            Active = userModel.Active
        };
    }

    // Mapea UserModel -> DataUserDto (para respuesta de Login SIN password)
    public static DataUserDto ToDataUserDto (this UserModel userModel)
    {
        return new DataUserDto
        {
            Id = userModel.Id,
            Name = userModel.Name,
            LastName = userModel.LastName,
            PhoneNumber = userModel.PhoneNumber,
            Email = userModel.Email,
            // Password NO se incluye
            DepartmentId = userModel.DepartmentId,
            Role = userModel.Role
        };
    }

    
    // public static UserModel ToModel (this RegisterUserDto registerUserDto)
    // {
    //     return new UserModel(
    //         0,
    //         registerUserDto.Name,
    //         registerUserDto.LastName,
    //         registerUserDto.PhoneNumber,
    //         registerUserDto.Email,
    //         registerUserDto.Password,
    //         registerUserDto.DepartmentId,
    //         registerUserDto.Role,
    //         true
    //     );
    // }
    //
    // public static UserModel ToModel (this UserDto userDto)
    // {
    //     return new UserModel(
    //         userDto.Id,
    //         userDto.Name,
    //         userDto.LastName,
    //         userDto.PhoneNumber,
    //         userDto.Email,
    //         userDto.Password,
    //         userDto.DepartmentId,
    //         userDto.Role,
    //         userDto.Active
    //     );
    // }
}
