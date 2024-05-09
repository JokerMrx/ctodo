using Ctodo.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Ctodo.Data;
using Ctodo.Models;

namespace Ctodo.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ApplicationContext _applicationContext;

    public CategoryController(ILogger<CategoryController> logger, ApplicationContext applicationContext)
    {
        _logger = logger;
        _applicationContext = applicationContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var categories = _applicationContext.Categories.ToList();

        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var category = new Category() { Name = model.Name };

        _applicationContext.Categories.Add(category);
        await _applicationContext.SaveChangesAsync();

        return RedirectToActionPermanent("Index", "Category");
    }
}