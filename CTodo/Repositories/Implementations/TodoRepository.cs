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
                             JOIN CategoryTodo ct ON todo.TodoId = ct.TodosTodoId
                             JOIN Categories category ON ct.CategoriesCategoryId = category.CategoryId
                             ORDER BY todo.IsCompleted DESC
                             """;

        using var connection = _dataContext.CreateConnection();
        var todos = await connection.QueryAsync<Todo, Category, Todo>(query, (todo, category) =>
        {
            todo.Categories.Add(category);
            return todo;
        }, splitOn: "CategoryId");

        return todos;
    }

    public async Task<Todo> GetById(int todoId)
    {
        const string query = """
                             SELECT * FROM Todos
                             WHERE TodoId = (@todoId)
                             """;

        using var connection = _dataContext.CreateConnection();
        var todo = await connection.QueryFirstOrDefaultAsync<Todo>(query, new { todoId });

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
                             INSERT INTO Todos (Title, Priority, IsCompleted, DueDate)
                             VALUES (@Title, @Priority, @IsCompleted, @DueDate)
                             SELECT CAST(SCOPE_IDENTITY() as int)
                             """;

        var category = await _categoryRepository.GetById(categoryId);

        using var connection = _dataContext.CreateConnection();
        var todoId = await connection.ExecuteScalarAsync<int>(query,
            new Todo()
            {
                Title = todoTitle, Priority = priority,
                DueDate = dueDate
            });

        if (category != null)
        {
            var linkQuery = """
                            INSERT INTO CategoryTodo (TodosTodoId, CategoriesCategoryId)
                            VALUES (@TodosTodoId, @CategoriesCategoryId);
                            """;
            await connection.ExecuteAsync(linkQuery,
                new { TodosTodoId = todoId, CategoriesCategoryId = category.CategoryId });
        }

        var todo = await GetById(todoId);

        return todo;
    }

    public async Task<Todo> ToggleCompleted(int todoId, bool isCompleted)
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

    public async Task<Todo> DeleteById(int todoId)
    {
        const string query = """
                             DELETE FROM Todos
                             OUTPUT DELETED.*
                             WHERE TodoId = @todoId
                             """;

        using var connection = _dataContext.CreateConnection();
        var todo = await connection.QuerySingleOrDefaultAsync<Todo>(query, new { todoId });

        if (todo == null) throw new Exception("This todo does not exist!");

        return todo;
    }
}