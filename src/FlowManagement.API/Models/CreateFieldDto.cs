using System.ComponentModel.DataAnnotations;

namespace FlowManagement.API.Models;

public class CreateFieldDto
{
  [Required]
  [RegularExpression(@"^F-\d{4}$", ErrorMessage = "The field code must be in the format F-XXXX")]
  public required string Code { get; set; }

  [Required]
  [StringLength(100)]
  public required string Name { get; set; }

  [Required]
  public required string DataType { get; set; } // "string", "int", "bool", etc.

  [StringLength(500)]
  public required string Description { get; set; }
}

public class UpdateFieldDto
{
  [Required]
  [StringLength(100)]
  public required string Name { get; set; }

  [StringLength(500)]
  public required string Description { get; set; }
}