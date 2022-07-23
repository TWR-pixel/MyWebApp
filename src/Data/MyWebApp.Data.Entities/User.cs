using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

public class User : EntityBase
{
    [Required] [StringLength(40)] public string Name { get; set; }
    [Required] public string Password { get; set; }
    [Required] public Role Role { get; set; }

    public ulong RoleId { get; set; }

    public User()
    {
    }

    public User(string name, string password, Role role)
    {
        Name = name;
        Password = password;
        Role = role;
    }
}