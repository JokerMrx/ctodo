using System.Xml;
using System.Xml.Linq;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using CTodo.Options;
using CTodo.Repositories.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CTodo.Repositories.Implementations;

public class TodoXmlRepository : ITodoRepository
{
    private readonly string _xmlStoragePath;
    private readonly ICategoryRepository _categoryRepository;

    public TodoXmlRepository(IOptions<XmlStorageOptions> options, ICategoryRepository categoryRepository)
    {
        _xmlStoragePath = options.Value.Path;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Todo>> Todos()
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);

            var todosElement = xmlDocument.Root.Element("Todos");

            if (todosElement == null)
            {
                Console.WriteLine("Todos element not found in XML file.");
                return new List<Todo>();
            }

            var todos = todosElement.Elements("Todo")
                .Select(todoElement =>
                {
                    var dueDateValue = todoElement.Element("DueDate")?.Value;
                    DateTime.TryParse(dueDateValue, out var dueDate);

                    return new Todo()
                    {
                        TodoId = new Guid(todoElement.Element("TodoId").Value),
                        Title = todoElement.Element("Title").Value,
                        Priority = todoElement.Element("Priority").Value,
                        IsCompleted = Convert.ToBoolean(todoElement.Element("IsCompleted").Value),
                        DueDate = !dueDateValue.IsNullOrEmpty() ? dueDate : null,
                    };
                })
                .OrderBy(t => t.IsCompleted).ToList();

            return todos;
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Error loading XML file: {ex.Message}");
            return [];
        }
    }

    public async Task<Todo> GetById(Guid todoId)
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);
            var xmlCategoriesRoot = xmlDocument.Root.Element("Todos");

            if (xmlCategoriesRoot == null) throw new Exception("This todo does not exist!");

            var todoElement = xmlCategoriesRoot.Elements("Todo")
                ?.FirstOrDefault(c => todoId.Equals(new Guid(c.Element("TodoId").Value)));

            if (todoElement == null) throw new Exception("This todo does not exist!");

            var dueDateValue = todoElement.Element("DueDate")?.Value;
            DateTime.TryParse(dueDateValue, out var dueDate);

            var todo = new Todo()
            {
                TodoId = new Guid(todoElement.Element("TodoId").Value),
                Title = todoElement.Element("Title").Value,
                Priority = todoElement.Element("Priority").Value,
                IsCompleted = Convert.ToBoolean(todoElement.Element("IsCompleted").Value),
                DueDate = !dueDateValue.IsNullOrEmpty() ? dueDate : null,
            };

            return todo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Todo> Create(TodoItemViewModel todoItemViewModel)
    {
        var todoTitle = todoItemViewModel.Title;
        var priority = todoItemViewModel.Priority;
        var categoryId = todoItemViewModel.Category;
        var dueDate = todoItemViewModel.DueDate;

        try
        {
            var category = await _categoryRepository.GetById(categoryId);

            if (category == null) throw new Exception("This category does not exist!");

            var todo = new Todo()
            {
                TodoId = Guid.NewGuid(),
                Title = todoTitle, Priority = priority,
                DueDate = dueDate
            };

            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);

            var xmlTodosRoot = xmlDocument.Root.Element("Todos") ?? new XElement("Todos");

            var categoryElement = new XElement("Category", new XElement("CategoryId", category.CategoryId),
                new XElement("Name", category.Name));

            xmlTodosRoot.Add(new XElement("Todo",
                new XElement("TodoId", todo.TodoId),
                new XElement("Title", todo.Title),
                new XElement("Priority", todo.Priority),
                new XElement("IsCompleted", todo.IsCompleted),
                new XElement("DueDate", todo.DueDate),
                categoryElement
            ));

            xmlDocument.Save(_xmlStoragePath);

            return todo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Todo> ToggleCompleted(Guid todoId, bool isCompleted)
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);
            var xmlCategoriesRoot = xmlDocument.Root.Element("Todos");

            if (xmlCategoriesRoot == null) throw new Exception("This todo does not exist!");

            var todoElement = xmlCategoriesRoot.Elements("Todo")
                .FirstOrDefault(c => todoId.Equals(new Guid(c.Element("TodoId").Value)));

            if (todoElement == null) throw new Exception("This todo does not exist!");

            var dueDateValue = todoElement.Element("DueDate")?.Value;
            DateTime.TryParse(dueDateValue, out var dueDate);

            var todo = new Todo()
            {
                TodoId = new Guid(todoElement.Element("TodoId").Value),
                Title = todoElement.Element("Title").Value,
                Priority = todoElement.Element("Priority").Value,
                IsCompleted = isCompleted,
                DueDate = !dueDateValue.IsNullOrEmpty() ? dueDate : null,
            };

            todoElement.Element("IsCompleted").Value = todo.IsCompleted.ToString();
            xmlDocument.Save(_xmlStoragePath);

            return todo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Todo> DeleteById(Guid todoId)
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);
            var xmlCategoriesRoot = xmlDocument.Root.Element("Todos");

            if (xmlCategoriesRoot == null) throw new Exception("This todo does not exist!");

            var todoElement = xmlCategoriesRoot.Elements("Todo")
                .FirstOrDefault(c => todoId.Equals(new Guid(c.Element("TodoId").Value)));

            if (todoElement == null) throw new Exception("This todo does not exist!");

            var dueDateValue = todoElement.Element("DueDate")?.Value;
            DateTime.TryParse(dueDateValue, out var dueDate);

            var todo = new Todo()
            {
                TodoId = new Guid(todoElement.Element("TodoId").Value),
                Title = todoElement.Element("Title").Value,
                Priority = todoElement.Element("Priority").Value,
                IsCompleted = Convert.ToBoolean(todoElement.Element("IsCompleted").Value),
                DueDate = !dueDateValue.IsNullOrEmpty() ? dueDate : null,
            };

            todoElement.Remove();
            xmlDocument.Save(_xmlStoragePath);

            return todo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}