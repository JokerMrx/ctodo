using CTodo.Providers;

namespace Ctodo.Models.ViewModel;

public class CategoryIndexViewModel
{
    public List<Category> Categories { get; set; }
    public CategoryViewModel CategoryCreate { get; set; }
}