using Domain.Dtos;
using Domain.Models;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IUserRepository: IGenericRepository<UserModel>
{
    Task<List<UserModel>> GetAllAsync();
    Task<List<UserDto>> GetUserByIdAsync(int id);
    Task<bool> IsUniqueUser(string user);
    Task<ResponseLoginUserDto> LoginAsync(LoginUserDto loginUserDto);
    Task<DataUserDto> RegisterAsync(RegisterUserDto registerUserDto);
}