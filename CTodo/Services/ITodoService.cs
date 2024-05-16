using Ctodo.Models;
using Ctodo.Models.ViewModel;

namespace CTodo.Services;

public interface ITodoService
{
    public List<Todo> GetTodos();
    public Task<Todo> GetTodoById(int todoId);
    public Task<Todo> CreateTodo(TodoItemViewModel todoItemViewModel);
    public Task<Todo> ToogleCompleted(int todoId, bool isCompleted);
    public Task<Todo> DeleteTodoById(int todoId);
}