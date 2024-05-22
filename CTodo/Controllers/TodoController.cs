using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Ctodo.Controllers;

public class TodoController : Controller
{
    private readonly ITodoRepository _todoRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TodoController(ITodoRepository todoRepository, ICategoryRepository categoryRepository)
    {
        _todoRepository = todoRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var todos = await _todoRepository.Todos();
        var categories = await _categoryRepository.Categories();

        var todoIndex = new TodoIndexViewModel()
        {
            Todos = todos.ToList(),
            Categories = categories.ToList(),
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

        await _todoRepository.Create(model);

        return RedirectToActionPermanent("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleTodoCompleted(int TodoId, bool IsCompleted)
    {
        await _todoRepository.ToggleCompleted(TodoId, IsCompleted);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTodo(int TodoId)
    {
        await _todoRepository.DeleteById(TodoId);

        return RedirectToAction("Index");
    }
}