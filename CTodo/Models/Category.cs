using System.ComponentModel.DataAnnotations;

namespace Ctodo.Models;

public class Category
{
    [Key] public Guid CategoryId { get; set; }
    [Required] public string? Name { get; set; }
}