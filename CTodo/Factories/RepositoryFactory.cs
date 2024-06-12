using CTodo.Options;
using CTodo.Repositories.Implementations;
using CTodo.Repositories.Infrastructure;
using Microsoft.Extensions.Options;

namespace CTodo.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<StorageOptions> _storageOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RepositoryFactory(IServiceProvider serviceProvider, IOptionsMonitor<StorageOptions> storageOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _serviceProvider = serviceProvider;
        _storageOptions = storageOptions;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetStorageType()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context != null && context.Request.Headers.TryGetValue("Database-type", out var storageType))
        {
            return storageType.ToString().ToLower();
        }

        return _storageOptions.CurrentValue.StorageType?.ToLower() ?? "database";
    }


    public ICategoryRepository CreateCategoryRepository()
    {
        var storageType = GetStorageType();

        switch (storageType.ToLower())
        {
            case "xml":
                return ActivatorUtilities.CreateInstance<CategoryXmlRepository>(_serviceProvider);
            case "database":
                return ActivatorUtilities.CreateInstance<CategoryRepository>(_serviceProvider);
            default:
                throw new ArgumentException("Invalid storage type");
        }
    }

    public ITodoRepository CreateTodoRepository()
    {
        var storageType = GetStorageType();

        switch (storageType.ToLower())
        {
            case "xml":
                return ActivatorUtilities.CreateInstance<TodoXmlRepository>(_serviceProvider);
            case "database":
                return ActivatorUtilities.CreateInstance<TodoRepository>(_serviceProvider);
            default:
                throw new ArgumentException("Invalid storage type");
        }
    }
}