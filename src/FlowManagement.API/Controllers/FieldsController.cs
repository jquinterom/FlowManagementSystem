using FlowManagement.API.Models;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FieldsController(IFieldService fieldService) : ControllerBase
{
  private readonly IFieldService _fieldService = fieldService;

  [HttpGet]
  public async Task<IActionResult> GetAllFields()
  {
    var fields = await _fieldService.GetAllFieldsAsync();
    return Ok(fields);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetField(Guid id)
  {
    var field = await _fieldService.GetFieldAsync(id);
    return field != null ? Ok(field) : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> CreateField([FromBody] CreateFieldDto dto)
  {
    try
    {
      var field = await _fieldService.CreateFieldAsync(
          dto.Code,
          dto.Name,
          dto.DataType,
          dto.Description);

      return CreatedAtAction(
          nameof(GetField),
          new { id = field.Id },
          field);
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateField(Guid id, [FromBody] UpdateFieldDto dto)
  {
    try
    {
      await _fieldService.UpdateFieldAsync(id, dto.Name, dto.Description);
      return NoContent();
    }
    catch (KeyNotFoundException)
    {
      return NotFound();
    }
  }
}