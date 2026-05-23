namespace ComputerComponentsApi.DTOs;

public class PcComponentResponseDto
{
    public string ComponentCode { get; set; } = null!;
    public string ComponentName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Amount { get; set; }
}