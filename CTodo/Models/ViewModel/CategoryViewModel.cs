using System.ComponentModel.DataAnnotations;

namespace Ctodo.Models.ViewModel;

public class CategoryViewModel
{
    [Required(ErrorMessage = "Category name is required!")]
    public string Name { get; set; }
}