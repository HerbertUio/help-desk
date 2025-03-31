using Domain.Dtos;
using Domain.Models;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IUserRepository: IGenericRepository<UserModel>
{
    Task<List<UserDto>> GetAllAsync();
    Task<List<UserDto>> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetUserByName(string name);
    Task<bool> IsUniqueUser(string user);
    Task<ResponseLoginUserDto> LoginAsync(LoginUserDto loginUserDto);
}