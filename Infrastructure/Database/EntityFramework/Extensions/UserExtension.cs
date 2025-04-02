using Domain.Dtos;
using Domain.Models;
using Infrastructure.Database.EntityFramework.Entities;

namespace Infrastructure.Database.EntityFramework.Extensions;

public static class UserExtension
{
    public static UserEntity ToEntity(this RegisterUserDto dto)
    {
        return new UserEntity()
        {
            Name = dto.Name,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Password = dto.Password,
            DepartmentId = dto.DepartmentId,
            Role = dto.Role,
            Active = true
        };
    }

    public static UserEntity ToEntity(this UserModel model)
    {
        return new UserEntity()
        {
            Id = model.Id,
            Name = model.Name,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Password = model.Password,
            DepartmentId = model.DepartmentId,
            Role = model.Role,
            Active = model.Active
        };
    }
    
    public static UserModel ToModel(this UserEntity entity)
    {
        return new UserModel(
            entity.Id,
            entity.Name,
            entity.LastName,
            entity.PhoneNumber,
            entity.Email,
            entity.Password,
            entity.DepartmentId,
            entity.Role,
            entity.Active);
    }
    
}