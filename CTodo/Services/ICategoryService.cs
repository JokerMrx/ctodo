using Ctodo.Models;
using Ctodo.Models.ViewModel;

namespace CTodo.Services;

public interface ICategoryService
{
    public List<Category> GetCategories();
    public Task<Category> CreateCategory(CategoryViewModel categoryViewModel);
    public Task<Category> GetCategoryById(int categoryId);
    public Task<Category> DeleteCategoryById(int categoryId);
}