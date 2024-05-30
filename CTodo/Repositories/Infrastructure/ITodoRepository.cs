using Ctodo.Models;
using Ctodo.Models.ViewModel;

namespace CTodo.Repositories.Infrastructure;

public interface ITodoRepository
{
    public Task<IEnumerable<Todo>> Todos();
    public Task<Todo> GetById(Guid todoId);
    public Task<Todo> Create(TodoItemViewModel todoItemViewModel);
    public Task<Todo> ToggleCompleted(Guid todoId, bool isCompleted);
    public Task<Todo> DeleteById(Guid todoId);
}