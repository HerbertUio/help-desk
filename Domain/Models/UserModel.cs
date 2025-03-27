using Domain.Models.Common;

namespace Domain.Models;

public class UserModel: BaseModel
{
    public string Name {get; set;}
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool Active { get; set; }
    
    public UserModel(
        int id,
        string name,
        string lastName,
        string email,
        string password,
        string role,
        bool active) : base(id)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = role;
        Active = active;
    }
}