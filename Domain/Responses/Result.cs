using System.Net;

namespace Domain.Responses;

public class Result<T>
{
    public T Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public List<string> Errors { get; private set; }
    
    private Result(T data, bool isSuccess, HttpStatusCode code, string message, List<string> errors)
    {
        Data = data;
        IsSuccess = isSuccess;
        StatusCode = code;
        Message = message ?? string.Empty;
        Errors = errors ?? new List<string>();
    }

    // Constructor para resultados exitosos por defecto
    public Result()
    {
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
        Errors = new List<string>();
    }

    // Constructor para errores con un mensaje
    public Result(string errorMessage)
    {
        IsSuccess = false;
        Message = errorMessage;
        StatusCode = HttpStatusCode.BadRequest;
        Errors = new List<string> { errorMessage };
    }

    // Constructor con código HTTP y mensaje opcional
    public Result(HttpStatusCode statusCode, string message = null)
    {
        IsSuccess = statusCode == HttpStatusCode.OK;
        StatusCode = statusCode;
        Message = message ?? string.Empty;
        Errors = IsSuccess ? new List<string>() : new List<string> { message };
    }

    // Método estático para resultados exitosos
    public static Result<T> Success(T data, HttpStatusCode code = HttpStatusCode.OK, string message = null)
    {
        return new Result<T>(
            data,
            true,
            code,
            message ?? HttpStatusMessages.GetMessage((int)code),
            new List<string>()
        );
    }

    // Método estático para resultados fallidos
    public static Result<T> Failure(List<string> errors, HttpStatusCode code)
    {
        return new Result<T>(
            default(T),
            false,
            code,
            HttpStatusMessages.GetMessage((int)code),
            errors
        );
    }

    // Método estático SuccessS3Result
    public static Result<T> SuccessS3Result(T data, string message = null)
    {
        return new Result<T>(
            data,
            true,
            HttpStatusCode.OK,
            message,
            new List<string>()
        );
    }

    // Método estático ErrorS3Result
    public static Result<T> ErrorS3Result(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(
            default(T),
            false,
            statusCode,
            errorMessage,
            new List<string> { errorMessage }
        );
    }

    // Sobrecarga para éxito con mensaje sin HttpStatusCode explícito
    public static Result<T> Success(T data, string message)
    {
        return new Result<T>(
            data,
            true,
            HttpStatusCode.OK,
            message,
            new List<string>()
        );
    }

    // Sobrecarga para fallo con lista de errores y mensaje
    public static Result<T> Failure(List<string> errors, string message)
    {
        return new Result<T>(
            default(T),
            false,
            HttpStatusCode.BadRequest,
            message,
            errors
        );
    }
}