using Microsoft.AspNetCore.Mvc;
using SourcefitClone.Api.DTOs;
using SourcefitClone.Api.Services;

namespace SourcefitClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(EmployeeService employeeService) : ControllerBase
{
    private readonly EmployeeService _employeeService = employeeService;

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponseDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponseDto>> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee is null) return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDto>> Create(EmployeeCreateDto dto)
    {
        var created = await _employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeResponseDto>> Update(int id, EmployeeUpdateDto dto)
    {
        var updated = await _employeeService.UpdateAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}