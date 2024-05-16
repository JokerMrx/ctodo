using Ctodo.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using CTodo.Services;

namespace Ctodo.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly CategoryService _categoryService;

    public CategoryController(ILogger<CategoryController> logger, CategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var categories = _categoryService.GetCategories();

        var categoryIndexViewModel = new CategoryIndexViewModel()
        {
            Categories = categories
        };
        
        return View(categoryIndexViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        await _categoryService.CreateCategory(model);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int categoryId)
    {
        await _categoryService.DeleteCategoryById(categoryId);
        
        return RedirectToAction("Index");
    }
}