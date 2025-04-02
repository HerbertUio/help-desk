using Domain.Dtos;
using Domain.Models;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IUserRepository: IGenericRepository<UserModel>
{
    Task<List<UserModel>> GetAllAsync();
    Task<UserModel?> GetUserByIdAsync(int id);
    Task<UserModel?> GetUserByEmailAsync(string email);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> DeleteUserByIdAsync(int id);
}