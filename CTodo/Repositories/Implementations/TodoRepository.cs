using Ctodo.Data;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using Dapper;

namespace CTodo.Repositories.Implementations;

public class TodoRepository : ITodoRepository
{
    private readonly DataContext _dataContext;
    private readonly ICategoryRepository _categoryRepository;

    public TodoRepository(DataContext dataContext, ICategoryRepository categoryRepository)
    {
        _dataContext = dataContext;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Todo>> Todos()
    {
        const string query = """
                             SELECT * FROM Todos todo
                             JOIN CategoryTodo ct ON todo.TodoId = ct.TodoId
                             JOIN Categories category ON ct.CategoryId = category.CategoryId
                             ORDER BY todo.IsCompleted
                             """;

        using var connection = _dataContext.CreateConnection();
        var todos = await connection.QueryAsync<Todo, Category, Todo>(query, (todo, category) =>
        {
            todo.Categories.Add(category);
            return todo;
        }, splitOn: "CategoryId");

        return todos;
    }

    public async Task<Todo> GetById(Guid todoId)
    {
        const string query = """
                             SELECT todo.*, category.CategoryId, category.Name
                             FROM Todos todo
                             JOIN CategoryTodo ct ON todo.TodoId = ct.TodoId
                             JOIN Categories category ON ct.CategoryId = category.CategoryId
                             WHERE todo.TodoId = @todoId
                             """;

        using var connection = _dataContext.CreateConnection();
        var todoDictionary = new Dictionary<Guid, Todo>();
        await connection.QueryAsync<Todo, Category, Todo>(query, (todo, category) =>
        {
            if (!todoDictionary.TryGetValue(todo.TodoId, out var currentTodo))
            {
                currentTodo = todo;
                todoDictionary.Add(currentTodo.TodoId, currentTodo);
            }

            currentTodo.Categories.Add(category);

            return currentTodo;
        }, new { todoId }, splitOn: "CategoryId");

        var todo = todoDictionary.Values.FirstOrDefault();

        if (todo == null) throw new Exception("This todo does not exist!");

        return todo;
    }

    public async Task<Todo> Create(TodoItemViewModel todoItemViewModel)
    {
        var todoTitle = todoItemViewModel.Title;
        var priority = todoItemViewModel.Priority;
        var categoryId = todoItemViewModel.Category;
        var dueDate = todoItemViewModel.DueDate;

        const string query = """
                             INSERT INTO Todos (TodoId, Title, Priority, IsCompleted, DueDate)
                             OUTPUT INSERTED.TodoId
                             VALUES (@TodoId, @Title, @Priority, @IsCompleted, @DueDate)
                             """;

        var category = await _categoryRepository.GetById(categoryId);

        using var connection = _dataContext.CreateConnection();
        var todoId = await connection.ExecuteScalarAsync<Guid>(query,
            new Todo()
            {
                TodoId = Guid.NewGuid(),
                Title = todoTitle, Priority = priority,
                DueDate = dueDate
            });

        if (category != null)
        {
            var linkQuery = """
                            INSERT INTO CategoryTodo (Id, TodoId, CategoryId)
                            VALUES (@Id, @TodoId, @CategoryId);
                            """;
            await connection.ExecuteAsync(linkQuery,
                new { Id = Guid.NewGuid(), TodoId = todoId, CategoryId = category.CategoryId });
        }

        var todo = await GetById(todoId);

        return todo;
    }

    public async Task<Todo> ToggleCompleted(Guid todoId, bool isCompleted)
    {
        const string query = """
                             UPDATE Todos SET IsCompleted = @IsCompleted
                             WHERE TodoId = @TodoId
                             """;

        var connection = _dataContext.CreateConnection();
        await connection.ExecuteAsync(query, new { TodoId = todoId, IsCompleted = isCompleted });

        var todo = await GetById(todoId);

        return todo;
    }

    public async Task<Todo> DeleteById(Guid todoId)
    {
        const string query = """
                             DELETE FROM Todos
                             OUTPUT DELETED.*
                             WHERE TodoId = @todoId
                             """;

        const string queryCategoryTodo = """
                                         DELETE FROM CategoryTodo
                                         WHERE TodoId = @todoId
                                         """;

        using var connection = _dataContext.CreateConnection();
        await connection.ExecuteAsync(queryCategoryTodo, new { todoId });
        var todo = await connection.QuerySingleOrDefaultAsync<Todo>(query, new { todoId });

        if (todo == null) throw new Exception("This todo does not exist!");

        return todo;
    }
}