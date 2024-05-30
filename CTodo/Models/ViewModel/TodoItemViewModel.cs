using System.ComponentModel.DataAnnotations;

namespace Ctodo.Models.ViewModel;

public class TodoItemViewModel
{
    [Required(ErrorMessage = "Title is required!")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Category is required!")]
    public Guid Category { get; set; }

    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
}