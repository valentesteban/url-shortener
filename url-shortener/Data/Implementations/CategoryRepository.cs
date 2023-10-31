using url_shortener.Data;
using url_shortener.Entities;
using url_shortener.Models.Repository.Interface;

namespace url_shortener.Models.Repository.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly UrlShortenerContext _context;
    
    public CategoryRepository(UrlShortenerContext context)
    {
        _context = context;
    }
    
    public List<Category> getAll()
    {
        return _context.Categories.ToList();
    }

    public Category getById(int id)
    {
       var category = _context.Categories.FirstOrDefault((category) => category.Id == id);
        
        if (category == null)
        {
            throw APIException.CreateException(APIException.Code.CT_01, "Category not found",
                APIException.Type.NOT_FOUND);
        }

        return category;
    }
    
    public Category? getByName(string name)
    {
        var category = _context.Categories.FirstOrDefault((category) => category.Name.ToLower() == name.ToLower());
        
        if (category == null)
        {
            throw APIException.CreateException(APIException.Code.CT_01, "Category not found",
                APIException.Type.NOT_FOUND);
        }

        return category;
    }

    public void createCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw APIException.CreateException(APIException.Code.CT_03, "Name is required",
                APIException.Type.BAD_REQUEST);
        }
        
        if (getByName(name) != null)
        {
            throw APIException.CreateException(APIException.Code.CT_02, "Category already exist",
                APIException.Type.BAD_REQUEST);
        }
        
        
        var category = new Category
        {
            Name = name.ToLower()
        };

        try
        {
            _context.Categories.Add(category);
        }
        catch (Exception e)
        {
            throw APIException.CreateException(APIException.Code.DB_01, e.Message,
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
        
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(APIException.Code.DB_02, e.Message,
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }

    public void updateCategory(int id, string name)
    {
        Category? category = getById(id);

        category.Name = name.ToLower();
            
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(APIException.Code.DB_02, e.Message,
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }

    public void deleteCategory(int id)
    {
        Category? category = getById(id);
        
        try
        {
            _context.Categories.Remove(category);
        }
        catch (Exception e)
        {
            throw APIException.CreateException(APIException.Code.DB_01, e.Message,
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
        
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(APIException.Code.DB_02, e.Message,
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }
}