using Application.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    internal static void MapAuthEndpoints(this WebApplication webApp)
    {
        var group = webApp.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", HandleLogin)
            .WithName("LoginUser")
            .Accepts<LoginUserDto>("application/json")
            .Produces<ResponseLoginUserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Autentica un usuario y devuelve un token JWT.",
                Description = "Recibe las credenciales del usuario (email, password) " +
                              "y devuelve los datos del usuario y un token si la autenticación es exitosa."
            });
    }
    private static async Task<IResult> HandleLogin(
        [FromBody] LoginUserDto loginDto,
        UserService userService, 
        ILogger logger) 
    {
        try
        {
            var result = await userService.Login(loginDto);

            if (!result.IsSuccess)
            {
                logger
                    .LogWarning("Intento de login fallido para email: {Email}. Motivo: {Motivo}"
                        , loginDto.Email, string.Join(", ", result.Errors));
                return Results.Unauthorized();
            }
            logger.LogInformation("Login exitoso para email: {Email}", loginDto.Email);
            return Results.Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger
                .LogError(ex, "Error inesperado durante el proceso de login para email: {Email}", loginDto.Email);
            return Results
                .Problem("Ocurrió un error inesperado durante el login.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}