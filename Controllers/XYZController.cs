using Microsoft.AspNetCore.Mvc;
using url_shortener.Models;
using url_shortener.Models.Repository.Interface;
using url_shortener.Utilities;

namespace url_shortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XYZController : ControllerBase
{
    private readonly IXYZService _xyzContext;
    private readonly ICategoryService _categoryContext;
    private readonly APIException _apiException;

    public XYZController(IXYZService xyzContext, ICategoryService categoryContext, APIException apiException)
    {
        _xyzContext = xyzContext;
        _categoryContext = categoryContext;
        _apiException = apiException;
    }

    [Route("all")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_xyzContext.GetAll());
    }

    [Route("getLong")]
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
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
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
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
    
    [Route("deleteById")]
    [HttpDelete]
    public IActionResult DeleteUrl(int id)
    {
        try
        {
            _xyzContext.deleteUrl(id);
            return Ok("Url " + id + " deleted");
        }catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
    
    [Route("deleteByShort")]
    [HttpDelete]
    public IActionResult DeleteUrl(string urlShort)
    {
        try
        {
            _xyzContext.deleteUrl(urlShort);
            return Ok("Url " + urlShort + " deleted");   
        }catch (Exception e)
        {
            Enum.TryParse(e.Data["type"].ToString(), out APIException.Type type);

            return _apiException.getResultFromError(type, e.Data);
        }
    }
}