using System.ComponentModel.DataAnnotations;

namespace CheckMate2.Api.Models;

public class Checklist
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}
