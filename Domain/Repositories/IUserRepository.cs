using Domain.Dtos;
using Domain.Models;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IUserRepository: IGenericRepository<UserModel>
{
    Task<List<UserModel>> GetAllAsync();
}