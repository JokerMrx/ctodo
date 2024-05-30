using Ctodo.Models.ViewModel;
using CTodo.Options;
using CTodo.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ctodo.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOptionsMonitor<StorageOptions> _storageOptions;

    public CategoryController(ICategoryRepository categoryRepository, IOptionsMonitor<StorageOptions> storageOptions)
    {
        _categoryRepository = categoryRepository;
        _storageOptions = storageOptions;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.Categories();
        var storageOptions = _storageOptions.CurrentValue;
        ViewBag.CurrentStorageType = storageOptions.StorageType;

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
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        await _categoryRepository.DeleteById(categoryId);

        return RedirectToAction("Index");
    }
}