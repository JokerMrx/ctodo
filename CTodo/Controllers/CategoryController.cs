using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Ctodo.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.Categories();

        var categoryIndexViewModel = new CategoryIndexViewModel()
        {
            Categories = categories.ToList()
        };
        
        return View(categoryIndexViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        await _categoryRepository.Create(model);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int categoryId)
    {
        await _categoryRepository.DeleteById(categoryId);
        
        return RedirectToAction("Index");
    }
}