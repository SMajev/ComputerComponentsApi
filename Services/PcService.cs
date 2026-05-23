using ComputerComponentsApi.Data;
using ComputerComponentsApi.DTOs;
using ComputerComponentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerComponentsApi.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PcResponseDto>> GetAllAsync()
    {
        return await _context.Pcs
            .Select(pc => new PcResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PcComponentResponseDto>?> GetComponentsAsync(int id)
    {
        var exists = await _context.Pcs.AnyAsync(pc => pc.Id == id);

        if (!exists)
            return null;

        return await _context.PcComponents
            .Where(pc => pc.PcId == id)
            .Include(pc => pc.Component)
                .ThenInclude(c => c.ComponentManufacturer)
            .Include(pc => pc.Component)
                .ThenInclude(c => c.ComponentType)
            .Select(pc => new PcComponentResponseDto
            {
                ComponentCode = pc.ComponentCode,
                ComponentName = pc.Component.Name,
                Description = pc.Component.Description,
                Manufacturer = pc.Component.ComponentManufacturer.FullName,
                Type = pc.Component.ComponentType.Name,
                Amount = pc.Amount
            })
            .ToListAsync();
    }

    public async Task<PcResponseDto> CreateAsync(PcRequestDto dto)
    {
        var pc = new Pc
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.Pcs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> UpdateAsync(int id, PcRequestDto dto)
    {
        var pc = await _context.Pcs.FindAsync(id);

        if (pc is null)
            return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.Pcs.FindAsync(id);

        if (pc is null)
            return false;

        _context.Pcs.Remove(pc);
        await _context.SaveChangesAsync();

        return true;
    }
}