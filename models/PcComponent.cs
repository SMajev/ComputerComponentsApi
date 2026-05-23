using System.ComponentModel;

namespace ComputerComponentsApi.Models;

public class PcComponent
{
    public int PcId { get; set; }
    public string ComponentCode { get; set; } = null!;
    public int Amount { get; set; }

    public Pc Pc { get; set; } = null!;
    public Component Component { get; set; } = null!;
}