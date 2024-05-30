using CTodo.Repositories.Infrastructure;

namespace CTodo.Factories;

public interface IRepositoryFactory
{
    ICategoryRepository CreateCategoryRepository();
    ITodoRepository CreateTodoRepository();
}