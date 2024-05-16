using Ctodo.Data;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CTodo.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationContext _applicationContext;

    public CategoryService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public List<Category> GetCategories()
    {
        var categories = _applicationContext.Categories.ToList();

        return categories;
    }

    public async Task<Category> CreateCategory(CategoryViewModel categoryViewModel)
    {
        var categoryName = categoryViewModel.Name;

        var category = new Category()
        {
            Name = categoryName
        };
        
        _applicationContext.Categories.Add(category);
        await _applicationContext.SaveChangesAsync();

        return category;
    }

    public async Task<Category> GetCategoryById(int categoryId)
    {
        var category = await _applicationContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        if (category == null) throw new Exception("Such a category does not exist!");
        
        return category;
    }

    public async Task<Category> DeleteCategoryById(int categoryId)
    {
        var category = await GetCategoryById(categoryId);

        _applicationContext.Categories.Remove(category);
        await _applicationContext.SaveChangesAsync();
        
        return category;
    }
}