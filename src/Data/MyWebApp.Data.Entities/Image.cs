using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Data.Entities;

public record Image : EntityBase
{
    public string? Url { get; set; }
    public string? Path { get; set; }
    [Required] public bool IsPublished { get; set; }
    public ulong GroupId { get; set; }

    public Image(string? url, string? path, bool isPublished, ulong groupId)
    {
        Url = url;
        Path = path;
        IsPublished = isPublished;
        GroupId = groupId;
    }

    public Image()
    {
    }
}