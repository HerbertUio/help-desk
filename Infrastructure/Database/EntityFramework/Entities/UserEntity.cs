using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Database.EntityFramework.Entities.Common;

namespace Infrastructure.Database.EntityFramework.Entities;

[Table("Users", Schema = "CTR")]
public class UserEntity: BaseEntity, IIdentifiable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }
    
    [Required]
    [Column("last_name", TypeName = "varchar(50)")]
    public string LastName { get; set; }
    
    [Required]
    [Column("phone_number", TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [Column("email", TypeName = "varchar(100)")]
    public string Email { get; set; }
    
    [Required]
    [Column("password", TypeName = "varchar(500)")]
    public string Password { get; set; }
    
    [Required] 
    [Column("department_id")]
    public int DepartmentId { get; set; }
    
    [Required]
    [Column("role", TypeName = "varchar(50)")]
    public string Role { get; set; }
    
    [Required]
    [Column("active", TypeName = "boolean")]
    public bool Active { get; set; }
}