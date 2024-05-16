using Ctodo.Data;
using Ctodo.Models.ViewModel;
using CTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ctodo.Controllers;

public class TodoController : Controller
{
    private readonly ILogger<TodoController> _logger;
    private readonly TodoService _todoService;
    private readonly CategoryService _categoryService;

    public TodoController(ILogger<TodoController> logger, TodoService todoService, CategoryService categoryService)
    {
        _logger = logger;
        _todoService = todoService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var todos = _todoService.GetTodos();
        var categories = _categoryService.GetCategories();

        var todoIndex = new TodoIndexViewModel()
        {
            Todos = todos,
            Categories = categories,
        };

        return View(todoIndex);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToActionPermanent("Index");
        }

        await _todoService.CreateTodo(model);

        return RedirectToActionPermanent("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToogleTodoCompleted(int TodoId, bool IsCompleted)
    {
        await _todoService.ToogleCompleted(TodoId, IsCompleted);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTodo(int TodoId)
    {
        await _todoService.DeleteTodoById(TodoId);

        return RedirectToAction("Index");
    }
}