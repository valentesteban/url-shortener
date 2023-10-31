using Microsoft.AspNetCore.Mvc;
using url_shortener.Models;
using url_shortener.Models.Repository.Interface;

namespace url_shortener.Controllers;

[ApiController]
[Route("{urlShort}")]
public class RedirectController : ControllerBase
{
    private readonly IXYZRepository _context;

    public RedirectController(IXYZRepository context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult getRedirect(string urlShort)
    {
        if (string.IsNullOrWhiteSpace(urlShort))
        {
            return BadRequest("Url short is required");
        }

        var urlLongByShort = _context.getUrlLongByShort(urlShort);
        
        if (urlLongByShort == null)
        {
            return NotFound();
        }

        _context.addClick(urlLongByShort.Id);
        
        return RedirectPermanent(urlLongByShort.UrlLong);
    }
}