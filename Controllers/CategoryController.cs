using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Interfaces;
using url_shortener.Utilities;

namespace url_shortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _context;
    private readonly ErrorSwitcher _errorSwitcher;

    public CategoryController(ICategoryService context, ErrorSwitcher errorSwitcher)
    {
        _context = context;
        _errorSwitcher = errorSwitcher;
    }

    [Route("all")]
    [HttpGet]
    public IActionResult getAll()
    {
        return Ok(_context.getAll());
    }

    [Route("id")]
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
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("name")]
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
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("create")]
    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult createCategory(string name)
    {
        try
        {
            _context.createCategory(name);

            return Ok("Category " + name + " created");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("update")]
    [Authorize(Roles = "admin")]
    [HttpPut]
    public IActionResult updateCategory(int id, string name)
    {
        try
        {
            _context.updateCategory(id, name);
            return Ok("Category " + name + " updated");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("delete")]
    [Authorize(Roles = "admin")]
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
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }
}