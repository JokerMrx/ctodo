using System.Xml;
using System.Xml.Linq;
using Ctodo.Models;
using Ctodo.Models.ViewModel;
using CTodo.Options;
using CTodo.Repositories.Infrastructure;
using Microsoft.Extensions.Options;

namespace CTodo.Repositories.Implementations;

public class CategoryXmlRepository : ICategoryRepository
{
    private readonly string _xmlStoragePath;

    public CategoryXmlRepository(IOptions<XmlStorageOptions> options)
    {
        _xmlStoragePath = options.Value.Path;
    }

    public async Task<IEnumerable<Category>> Categories()
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);

            var categoriesElement = xmlDocument.Root.Element("Categories");

            if (categoriesElement == null)
            {
                Console.WriteLine("Categories element not found in XML file.");
                return new List<Category>();
            }

            var categories = categoriesElement.Elements("Category")
                .Select(categoryElement => new Category
                {
                    CategoryId = new Guid(categoryElement.Element("CategoryId").Value),
                    Name = categoryElement.Element("Name")?.Value
                })
                .ToList();

            return categories;
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Error loading XML file: {ex.Message}");
            return [];
        }
    }

    public async Task<Category> Create(CategoryViewModel categoryViewModel)
    {
        var categoryName = categoryViewModel.Name;
        var category = new Category() { CategoryId = Guid.NewGuid(), Name = categoryName };

        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);

            var xmlCategoriesRoot = xmlDocument.Root.Element("Categories") ?? new XElement("Categories");

            xmlCategoriesRoot.Add(new XElement("Category", new XElement("CategoryId", category.CategoryId),
                new XElement("Name", category.Name)));
            xmlDocument.Save(_xmlStoragePath);

            return category;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Category> GetById(Guid categoryId)
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);
            var xmlCategoriesRoot = xmlDocument.Root.Element("Categories");

            if (xmlCategoriesRoot == null) throw new Exception("This category does not exist!");

            var categoryElement = xmlCategoriesRoot.Elements("Category")
                ?.FirstOrDefault(c => categoryId.Equals(new Guid(c.Element("CategoryId").Value)));

            if (categoryElement == null) throw new Exception("This category does not exist!");

            var category = new Category()
            {
                CategoryId = new Guid(categoryElement.Element("CategoryId").Value),
                Name = categoryElement.Element("Name")?.Value
            };

            return category;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Category> DeleteById(Guid categoryId)
    {
        try
        {
            XDocument xmlDocument = XDocument.Load(_xmlStoragePath);
            var xmlCategoriesRoot = xmlDocument.Root.Element("Categories");

            if (xmlCategoriesRoot == null) throw new Exception("This category does not exist!");

            var categoryElement = xmlCategoriesRoot.Elements("Category")
                .FirstOrDefault(c => categoryId.Equals(new Guid(c.Element("CategoryId").Value)));

            if (categoryElement == null) throw new Exception("This category does not exist!");

            var category = new Category()
            {
                CategoryId = new Guid(categoryElement.Element("CategoryId").Value),
                Name = categoryElement.Element("Name")?.Value
            };

            categoryElement.Remove();
            xmlDocument.Save(_xmlStoragePath);

            return category;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}