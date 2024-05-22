using Ctodo.Models;
using Ctodo.Models.ViewModel;

namespace CTodo.Repositories.Infrastructure;

public interface ITodoRepository
{
    public Task<IEnumerable<Todo>> Todos();
    public Task<Todo> GetById(int todoId);
    public Task<Todo> Create(TodoItemViewModel todoItemViewModel);
    public Task<Todo> ToggleCompleted(int todoId, bool isCompleted);
    public Task<Todo> DeleteById(int todoId);
}