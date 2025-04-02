using Domain.Models;
using Domain.Repositories;
using Infrastructure.Database.EntityFramework.Context;
using Infrastructure.Database.EntityFramework.Entities;
using Infrastructure.Database.EntityFramework.Exceptions;
using Infrastructure.Database.EntityFramework.Extensions;
using Infrastructure.Database.EntityFramework.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.EntityFramework.Repositories;

public class UserRepository: GenericRepository<UserEntity>, IUserRepository
{
    private readonly HelpDeskDbContext _context;
    public UserRepository(HelpDeskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserModel> CreateAsync(UserModel model)
    {
       var entity = model.ToEntity();
       var createdEntity = await base.CreateAsync(entity);
       await _context.SaveChangesAsync();
       return createdEntity.ToModel();
    }

    public async Task<UserModel> UpdateAsync(UserModel model)
    {
        var entity = model.ToEntity();
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public async Task<List<UserModel>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(user => user.ToModel()).ToList();
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        return entity?.ToModel();
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return entity?.ToModel();
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user == null;
    }

    public async Task<bool> DeleteUserByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException($"User with id {id} not found.");
        }
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}