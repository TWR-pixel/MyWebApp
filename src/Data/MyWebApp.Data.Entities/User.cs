using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

public record User : EntityBase
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required]
    [StringLength(40)]
    public string Name { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    [Required]
    public Role Role { get; set; }

    public ulong RoleId { get; set; }

    public User()
    {
    }

    public User(string name, string password, Role role)
    {
        Name = name;
        Password = password;
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }
}