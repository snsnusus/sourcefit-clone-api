using Microsoft.AspNetCore.Mvc;
using SourcefitClone.Api.DTOs;
using SourcefitClone.Api.Services;

namespace SourcefitClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(DepartmentService departmentService) : ControllerBase
{
    private readonly DepartmentService _departmentService = departmentService;

    [HttpGet]
    public async Task<ActionResult<List<DepartmentResponseDto>>> GetAll()
    {
        return Ok(await _departmentService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null) return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponseDto>> Create(DepartmentCreateDto dto)
    {
        var created = await _departmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> Update(int id, DepartmentCreateDto dto)
    {
        var updated = await _departmentService.UpdateAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}