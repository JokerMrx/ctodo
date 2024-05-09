using Ctodo.Data;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Ctodo.Controllers;

public class TodoController : Controller
{
    private readonly ILogger<TodoController> _logger;
    private readonly ApplicationContext _applicationContext;

    public TodoController(ILogger<TodoController> logger, ApplicationContext applicationContext)
    {
        _logger = logger;
        _applicationContext = applicationContext;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var todosNotCompleted = _applicationContext.Todos.Where(t => t.IsCompleted == false).Include(t => t.Categories)
            .AsEnumerable().OrderBy(t => t.Priority, new PriorityComparer()).ToList();
        var todosCompleted = _applicationContext.Todos.Where(t => t.IsCompleted == true).Include(t => t.Categories)
            .AsEnumerable().OrderBy(t => t.Priority, new PriorityComparer()).ToList();
        
        return View(new Tuple<List<Todo>, List<Todo>>(todosNotCompleted, todosCompleted));
    }

    [HttpGet]
    public IActionResult Create()
    {
        var categories = _applicationContext.Categories.ToList();

        return View(new Tuple<List<Category>, TodoItemViewModel?>(categories, null));
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = _applicationContext.Categories.ToList();

            return View(new Tuple<List<Category>, TodoItemViewModel>(categories, model));
        }

        string title = model.Title, priority = model.Priority;
        var categoryId = model.Category;
        var dueDate = model.DueDate;

        var category =
            await _applicationContext.Categories.FirstOrDefaultAsync(categor => categor.CategoryId == categoryId);

        if (category == null)
        {
            ModelState.AddModelError("Category", "Such a category does not exist!");
            return View();
        }

        var todo = new Todo()
            { Title = title, Priority = priority, DueDate = dueDate, Categories = new List<Category>() { category } };

        _applicationContext.Todos.Add(todo);
        await _applicationContext.SaveChangesAsync();

        return RedirectToActionPermanent("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToogleTodoCompleted(int TodoId, bool IsCompleted)
    {
        Console.WriteLine(TodoId);
        Console.WriteLine(IsCompleted);
        var todo = await _applicationContext.Todos.FirstOrDefaultAsync(t => t.TodoId == TodoId);

        if (todo == null) return NotFound();
        Console.WriteLine(todo.Title);
        todo.IsCompleted = IsCompleted;
        _applicationContext.Update(todo);

        await _applicationContext.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}