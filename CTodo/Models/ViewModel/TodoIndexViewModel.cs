namespace Ctodo.Models.ViewModel;

public class TodoIndexViewModel
{
    public List<Todo> Todos { get; set; }
    public List<Category> Categories { get; set; }
    public TodoItemViewModel? TodoItemViewModel { get; set; }
}