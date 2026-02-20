using System.ComponentModel.DataAnnotations;

namespace Knowledge_Base_API.Entity;

public class Note
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }
    
    public string Content { get; set; } = null!;
    
    public string Tags { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}