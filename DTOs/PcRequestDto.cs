using System.ComponentModel.DataAnnotations;

namespace ComputerComponentsApi.DTOs;

public class PcRequestDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Range(0.1, 9999)]
    public decimal Weight { get; set; }

    [Range(0, 120)]
    public int Warranty { get; set; }

    public DateTime CreatedAt { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}