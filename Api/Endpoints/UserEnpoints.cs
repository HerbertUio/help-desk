using Application.Services;
using Domain.Dtos;
using Domain.Models;

namespace Api.Endpoints;

public static class UserEnpoints
{
    internal static void MapUserEndpoints(this WebApplication webApp)
    {
        webApp.MapGroup("/users").WithTags("Users").MapGroupUsers();
    }

    internal static void MapGroupUsers(this RouteGroupBuilder group)
    {
        group.MapPost("", async (RegisterUserDto dto, UserService service) => await service.Create(dto));
        group.MapGet("", async (UserService service) => await service.GetAllAsync());
        group.MapGet("/{id}", async (int id, UserService service) => await service.GetUserByIdAsync(id));
        group.MapDelete("/{id}", async (int id, UserService service) => await service.DeleteUserAsync(id));
        group.MapPut("/{id}", async (int id, UserModel model, UserService service) => await service.UpdateUserAsync(id, model));
    }
}