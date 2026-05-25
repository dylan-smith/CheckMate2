using System.ComponentModel.DataAnnotations;

namespace CheckMate.Api.Contracts;

public class ChecklistRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}
