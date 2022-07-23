using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

public class Image : EntityBase
{
    public string? Url { get; set; }
    public string? Path { get; set; }

    [Required] public bool IsPublished { get; set; }

    public Group Group { get; set; }

    public Image(string? url, string? path, bool isPublished, Group group)
    {
        Url = url;
        Path = path;
        IsPublished = isPublished;
        Group = group;
    }

    public Image()
    {
    }
}