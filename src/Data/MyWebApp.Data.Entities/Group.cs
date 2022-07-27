using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

/// <summary>
/// Entity contains images
/// </summary>
public record Group : EntityBase
{
    /// <summary>
    /// Group name
    /// </summary>
    [StringLength(40)]
    [Required]
    public string? Name { get; set; }
    /// <summary>
    /// Group images
    /// </summary>
    public List<Image>? Images { get; set; }

    public Group(string? name, List<Image>? images)
    {
        Name = name;
        Images = images ?? throw new ArgumentNullException(nameof(images));
    }

    public Group()
    {

    }
}
