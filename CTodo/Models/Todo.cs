using System.ComponentModel.DataAnnotations;

namespace Ctodo.Models;

public class Todo
{
    [Key] public int TodoId { get; set; }
    [Required] public string Title { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? DueDate { get; set; }
    [MaxLength(50)] public string Priority { get; set; } = "Low";
    public List<Category> Categories { get; set; } = new List<Category>();
}