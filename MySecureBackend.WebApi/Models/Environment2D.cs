using System.ComponentModel.DataAnnotations;

namespace MySecureBackend.WebApi.Models;

public class Environment2D
{
    public string? Id { get; set; }
    
    [Required]
    [MaxLength(25)]
    [MinLength(1)]
    public string Name { get; set; }
    
    public int MaxHeight { get; set; }
    
    public int MaxLength { get; set; }
    public string? OwnerId { get; set; }
}