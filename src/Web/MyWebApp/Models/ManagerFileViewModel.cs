using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class ManagerFileViewModel
{
    [Required] public ulong Id { get; set; }
    [Required] public IFormFile? FormFile { get; set; }
        
    
}