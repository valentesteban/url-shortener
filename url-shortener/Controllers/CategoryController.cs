using Microsoft.AspNetCore.Mvc;
using url_shortener.Models;
using url_shortener.Models.Repository.Interface;

namespace url_shortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _context;
    private readonly APIException _apiException;
    
    public CategoryController(ICategoryRepository context, APIException apiException)
    {
        _context = context;
        _apiException = apiException;
    }
    
    [Route("all")]
    [HttpGet]
    public IActionResult getAll()
    {
        return Ok(_context.getAll());
    }
    
    [Route("getById")]
    [HttpGet]
    public IActionResult getById(int id)
    {
        try
        {
            var category = _context.getById(id);
            return Ok(category);
        }
        catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
    
    [Route("getByName")]
    [HttpGet]
    public IActionResult getByName(string name)
    {
        try
        {
            var category = _context.getByName(name);
            return Ok(category);
        }
        catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
    
    [Route("create")]
    [HttpPost]
    public IActionResult createCategory(string name)
    {
        try
        {
            _context.createCategory(name);
        
            return Ok("Category " + name +" created");   
        } catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
    
    [Route("update")]
    [HttpPut]
    public IActionResult updateCategory(int id, string name)
    {
        try
        {
            _context.updateCategory(id, name);
            return Ok("Category " + name +" updated");
        } catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }

    [Route("delete")]
    [HttpDelete]
    public IActionResult deleteCategory(int id)
    {
        try
        {
            _context.deleteCategory(id);

            return Ok("Category" + id + "deleted");
        }
        catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
}