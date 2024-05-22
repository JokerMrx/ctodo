using Ctodo.Models;
using Ctodo.Models.ViewModel;

namespace CTodo.Repositories.Infrastructure;

public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> Categories();
    public Task<Category> Create(CategoryViewModel categoryViewModel);
    public Task<Category> GetById(int categoryId);
    public Task<Category> DeleteById(int categoryId);
}