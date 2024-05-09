using System.ComponentModel.DataAnnotations;

namespace Ctodo.Models;

public class Category
{
    [Key] public int CategoryId { get; set; }
    [Required] public string Name { get; set; }
    public List<Todo> Todos { get; set; }
}