using url_shortener.Data;
using url_shortener.Entities;
using url_shortener.Models.Repository.Interface;
using url_shortener.Utilities;

namespace url_shortener.Data.Implementations;

public class CategoryService : ICategoryService
{
    private readonly UrlShortenerContext _context;

    public CategoryService(UrlShortenerContext context)
    {
        _context = context;
    }

    public List<Category> getAll()
    {
        return _context.Categories.ToList();
    }

    public Category? getById(int id)
    {
        var category = _context.Categories.FirstOrDefault((category) => category.Id == id);

        return category;
    }

    public Category? getByName(string name)
    {
        var category = _context.Categories.FirstOrDefault((category) => category.Name.ToLower() == name.ToLower());

        return category;
    }

    public void createCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("BD - Name is required");
        }


        if (_context.Categories.FirstOrDefault((category => category.Name == name)) != null)
        {
            throw new Exception("BD - Category already exists");
        }


        var category = new Category
        {
            Name = name.ToLower()
        };

        try
        {
            _context.Categories.Add(category);
        }
        catch (Exception exception)
        {
            throw new Exception("IE - An error occurred while setting the data in the database");
        }

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }

    public void updateCategory(int id, string name)
    {
        Category? category = getById(id);

        if (category == null)
        {
            throw new Exception("BD - Category not found");
        }

        category.Name = name.ToLower();

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }

    public void deleteCategory(int id)
    {
        Category? category = getById(id);

        if (category == null)
        {
            throw new Exception("BD - Category not found");
        }

        try
        {
            _context.Categories.Remove(category);
        }
        catch (Exception exception)
        {
            throw new Exception("IE - An error occurred while setting the data in the database");
        }

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }
}