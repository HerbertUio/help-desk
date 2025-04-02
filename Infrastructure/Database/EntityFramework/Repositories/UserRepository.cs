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
        if (model == null) throw new ArgumentNullException(nameof(model));
        var entity = model.ToEntity();
        if (entity.Id == 0)
        {
            var createdEntity = await base.CreateAsync(entity);
            await _context.SaveChangesAsync();
            return createdEntity.ToModel();
        }
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public async Task<UserModel> UpdateAsync(UserModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        var entity = model.ToEntity();
        if (entity.Id == 0)
        {
            var createdEntity = await base.CreateAsync(entity);
            await _context.SaveChangesAsync();
            return createdEntity.ToModel();
        }
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public async Task<List<UserModel>> GetAllAsync()
    {
        var entities = await _context.Users.ToListAsync();
        return entities.Select(u => u.ToModel()).ToList();
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(UserEntity), id);
        }

        try
        {
            var changes = await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return entity.ToModel();
            }
            else
            {
                throw new DbUpdateConcurrencyException("No se realizaron cambios en la base de datos.");
            }
        }
        catch (DbUpdateException ex) 
        {
            throw new DatabaseOperationException($"Error al eliminar usuario con ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<UserModel?> GetUserByNameAsync(string name)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        if (entity == null) return null;
        return entity.ToModel();
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (entity == null) return null;
        return entity.ToModel();
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return entity == null;
    }

    public async Task<bool> DeleteUserByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(UserEntity), id);
        }
        try
        {
            _context.Users.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
        catch (DbUpdateException ex)
        {
            throw new DatabaseOperationException($"Error al eliminar usuario con ID {id}: {ex.Message}", ex);
        }
    }
}