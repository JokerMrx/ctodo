using Ctodo.Data;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CTodo.Services;

public class TodoService : ITodoService
{
    private readonly ApplicationContext _applicationContext;
    private readonly CategoryService _categoryService;

    public TodoService(ApplicationContext applicationContext, CategoryService categoryService)
    {
        _applicationContext = applicationContext;
        _categoryService = categoryService;
    }

    public List<Todo> GetTodos()
    {
        var todos = _applicationContext.Todos.OrderBy(t => t.IsCompleted).Include(t => t.Categories).ToList();

        return todos;
    }

    public async Task<Todo> GetTodoById(int todoId)
    {
        var todo = await _applicationContext.Todos.FirstOrDefaultAsync(t => t.TodoId == todoId);

        return todo;
    }

    public async Task<Todo> CreateTodo(TodoItemViewModel todoItemViewModel)
    {
        string todoTitle = todoItemViewModel.Title, priority = todoItemViewModel.Priority;
        var dueDate = todoItemViewModel.DueDate;
        var categoryId = todoItemViewModel.Category;

        var category = await _categoryService.GetCategoryById(categoryId)!;

        var todo = new Todo()
        {
            Title = todoTitle,
            Categories = new List<Category>() { category },
            Priority = priority,
            DueDate = dueDate
        };

        _applicationContext.Todos.Add(todo);
        await _applicationContext.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> ToogleCompleted(int todoId, bool isCompleted)
    {
        var todo = await GetTodoById(todoId);

        todo.IsCompleted = isCompleted;
        _applicationContext.Update(todo);
        await _applicationContext.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> DeleteTodoById(int todoId)
    {
        var todo = await GetTodoById(todoId);
        
        _applicationContext.Todos.Remove(todo);
        await _applicationContext.SaveChangesAsync();

        return todo;
    }
}