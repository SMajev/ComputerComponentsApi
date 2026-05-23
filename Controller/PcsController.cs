using ComputerComponentsApi.DTOs;
using ComputerComponentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerComponentsApi.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PcsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PcResponseDto>>> GetAll()
    {
        var pcs = await _pcService.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    public async Task<ActionResult<IEnumerable<PcComponentResponseDto>>> GetComponents(int id)
    {
        var components = await _pcService.GetComponentsAsync(id);

        if (components is null)
            return NotFound();

        return Ok(components);
    }

    [HttpPost]
    public async Task<ActionResult<PcResponseDto>> Create([FromBody] PcRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdPc = await _pcService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetComponents), new { id = createdPc.Id }, createdPc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PcRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _pcService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pcService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}