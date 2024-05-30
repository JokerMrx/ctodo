using Ctodo.Data;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using Dapper;

namespace CTodo.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dataContext;

    public CategoryRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<Category>> Categories()
    {
        const string query = """
                             SELECT * FROM Categories
                             """;

        using var connection = _dataContext.CreateConnection();
        var categories = await connection.QueryAsync<Category>(query);

        return categories;
    }

    public async Task<Category> Create(CategoryViewModel categoryViewModel)
    {
        var categoryName = categoryViewModel.Name;

        const string query = """
                             INSERT INTO Categories (CategoryId, Name)
                             OUTPUT INSERTED.CategoryId
                             VALUES (@CategoryId, @Name)
                             """;

        using var connection = _dataContext.CreateConnection();
        var categoryId = await connection.ExecuteScalarAsync<Guid>(query,
            new Category() { CategoryId = Guid.NewGuid(), Name = categoryName });

        var category = await GetById(categoryId);

        return category;
    }

    public async Task<Category> GetById(Guid categoryId)
    {
        const string query = """
                             SELECT * FROM Categories
                             WHERE CategoryId = @categoryId
                             """;

        using var connection = _dataContext.CreateConnection();
        var category = await connection.QueryFirstOrDefaultAsync<Category>(query, new { categoryId });

        if (category == null) throw new Exception("This category does not exist!");

        return category;
    }

    public async Task<Category> DeleteById(Guid categoryId)
    {
        const string query = """
                             DELETE FROM Categories
                             OUTPUT DELETED.*
                             WHERE CategoryId = @categoryId;
                             """;

        using var connection = _dataContext.CreateConnection();
        var category = await connection.QuerySingleOrDefaultAsync<Category>(query, new { categoryId });

        if (category == null) throw new Exception("This category does not exist!");

        return category;
    }
}