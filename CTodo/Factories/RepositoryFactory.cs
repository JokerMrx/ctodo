using CTodo.Options;
using CTodo.Repositories.Implementations;
using CTodo.Repositories.Infrastructure;
using Microsoft.Extensions.Options;

namespace CTodo.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<StorageOptions> _storageOptions;

    public RepositoryFactory(IServiceProvider serviceProvider, IOptionsMonitor<StorageOptions> storageOptions)
    {
        _serviceProvider = serviceProvider;
        _storageOptions = storageOptions;
    }

    public ICategoryRepository CreateCategoryRepository()
    {
        var storageType = _storageOptions.CurrentValue.StorageType ?? "Database";

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
        var storageType = _storageOptions.CurrentValue.StorageType ?? "Database";

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