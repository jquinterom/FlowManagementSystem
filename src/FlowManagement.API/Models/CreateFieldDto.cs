using System.ComponentModel.DataAnnotations;

namespace FlowManagement.API.Models;

public class CreateFieldDto
{
  [Required]
  [RegularExpression(@"^F-\d{4}$", ErrorMessage = "The field code must be in the format F-XXXX")]
  public string Code { get; set; }

  [Required]
  [StringLength(100)]
  public string Name { get; set; }

  [Required]
  public string DataType { get; set; } // "string", "int", "bool", etc.

  [StringLength(500)]
  public string Description { get; set; }
}

public class UpdateFieldDto
{
  [Required]
  [StringLength(100)]
  public string Name { get; set; }

  [StringLength(500)]
  public string Description { get; set; }
}