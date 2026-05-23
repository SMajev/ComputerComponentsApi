using ComputerComponentsApi.DTOs;

namespace ComputerComponentsApi.Services;

public interface IPcService
{
    Task<IEnumerable<PcResponseDto>> GetAllAsync();
    Task<IEnumerable<PcComponentResponseDto>?> GetComponentsAsync(int id);
    Task<PcResponseDto> CreateAsync(PcRequestDto dto);
    Task<bool> UpdateAsync(int id, PcRequestDto dto);
    Task<bool> DeleteAsync(int id);
}