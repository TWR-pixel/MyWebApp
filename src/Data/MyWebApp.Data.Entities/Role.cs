using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

public record Role : EntityBase
{
    /// <summary>
    /// Название роли
    /// </summary>
    [Required] public string? Name { get; set; }
}