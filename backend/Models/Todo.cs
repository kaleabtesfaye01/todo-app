using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Todo
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public bool Completed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
} 