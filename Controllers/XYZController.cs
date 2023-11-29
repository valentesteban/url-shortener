using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Interfaces;
using url_shortener.Models;
using url_shortener.Utilities;

namespace url_shortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XYZController : ControllerBase
{
    private readonly IXYZService _xyzContext;
    private readonly ICategoryService _categoryContext;
    private readonly ErrorSwitcher _errorSwitcher;

    public XYZController(IXYZService xyzContext, ICategoryService categoryContext, ErrorSwitcher errorSwitcher)
    {
        _xyzContext = xyzContext;
        _categoryContext = categoryContext;
        _errorSwitcher = errorSwitcher;
    }

    [Route("all")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_xyzContext.GetAll());
    }

    [Route("long")]
    [HttpGet]
    public IActionResult GetUrlLongByShort(string urlShort)
    {
        try
        {
            var urlLongByShort = _xyzContext.getUrlLongByShort(urlShort);

            return Ok(urlLongByShort);
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
    [HttpPost]
    public IActionResult CreateUrl([FromBody] XYZForCreationDTO creationDto)
    {
        try
        {
            var url = _xyzContext.createUrl(creationDto);
            return Ok(url);
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("deleteId")]
    [Authorize(Roles = "admin")]
    [HttpDelete]
    public IActionResult DeleteUrl(int id)
    {
        try
        {
            _xyzContext.deleteUrl(id);
            return Ok("Url " + id + " deleted");
        }
        catch (Exception e)
        {
            var message = _errorSwitcher.GetErrorFromException(e.Message);

            var code = message.Item1;
            var msg = message.Item2;

            return _errorSwitcher.getResultFromError(code, msg);
        }
    }

    [Route("deleteShort")]
    [Authorize(Roles = "admin")]
    [HttpDelete]
    public IActionResult DeleteUrl(string urlShort)
    {
        try
        {
            _xyzContext.deleteUrl(urlShort);
            return Ok("Url " + urlShort + " deleted");
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