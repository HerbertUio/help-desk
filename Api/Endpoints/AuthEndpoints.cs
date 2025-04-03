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
            // ... (resto de la configuración del endpoint como antes) ...
            .WithName("LoginUser")
            .Accepts<LoginUserDto>("application/json")
            .Produces<ResponseLoginUserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(/*...*/);
    }

    private static async Task<IResult> HandleLogin(
            [FromBody] LoginUserDto loginDto,
            UserService userService,
            ILogger<AuthEndpoints> logger)
        // ---------------------------------------------------
    {
        try
        {
            var result = await userService.Login(loginDto);

            if (!result.IsSuccess)
            {
                logger.LogWarning("Intento de login fallido para {Email}. Errores: {Errors}", loginDto.Email, string.Join(", ", result.Errors));
                return Results.Unauthorized();
            }

            logger.LogInformation("Login exitoso para {Email}", loginDto.Email);
            return Results.Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado en HandleLogin para {Email}", loginDto.Email);
            return Results.Problem("Ocurrió un error inesperado.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}