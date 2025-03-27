namespace Domain.Dtos;

public class ResponseLoginUserDto
{
    public DataUserDto User { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}