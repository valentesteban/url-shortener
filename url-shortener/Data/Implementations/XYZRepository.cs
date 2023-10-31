using Microsoft.EntityFrameworkCore;
using url_shortener.Data;
using url_shortener.Entities;
using url_shortener.Models.Repository.Interface;
using url_shortener.Utilities;

namespace url_shortener.Data.Implementations;

public class XYZRepository : IXYZRepository
{
    private readonly UrlShortenerContext _context;

    public XYZRepository(UrlShortenerContext context)
    {
        _context = context;
    }
    public List<XYZ> GetAll()
    {
        return _context.Urls.ToList();
    }
    
    public XYZ getById(int id)
    {
        var url = _context.Urls.FirstOrDefault(url => url.Id == id);
        
        if (url == null)
        {
            throw APIException.CreateException(APIException.Code.URL_01, "Url not found",
                APIException.Type.NOT_FOUND);
        }
     
        return url;
    }
    
    public XYZ? getUrlLongByShort(string urlShort)
    {
        var url = _context.Urls.FirstOrDefault(url => url.UrlShort == urlShort);
        
        if (url == null)
        {
            throw APIException.CreateException(APIException.Code.URL_01, "Url not found",
                APIException.Type.NOT_FOUND);
        }
     
        return url;
    }
    
    public bool isUrlShortExist(string urlShort)
    {
        if (string.IsNullOrWhiteSpace(urlShort))
        {
            throw APIException.CreateException(APIException.Code.URL_03, "Url short is required",
                APIException.Type.BAD_REQUEST);
        }
        
        return _context.Urls.Any(url => url.UrlShort == urlShort);
    }
    
    public bool isUrlLongExist(string urlLong)
    {
        if (string.IsNullOrWhiteSpace(urlLong))
        {
            throw APIException.CreateException(APIException.Code.URL_02, "Url long is required",
                APIException.Type.BAD_REQUEST);
        }
        
        return _context.Urls.Any(url => url.UrlLong == urlLong);
    }
    
    public XYZ createUrl(XYZForCreationDto creationDto)
    {
        string randomUrl = urlGenerator.RandomString(6);
        
        while (isUrlShortExist(randomUrl))
        {
            randomUrl = urlGenerator.RandomString(6);
        }
        
        if (!Uri.IsWellFormedUriString(creationDto.UrlLong, UriKind.Absolute))
        {
            throw APIException.CreateException(APIException.Code.URL_04, "Url long is not valid",
                APIException.Type.BAD_REQUEST);
        }
        
        var url = new XYZ
        {
            Name = creationDto.Name,
            UrlLong = creationDto.UrlLong,
            UrlShort = randomUrl,
            CategoryId = _context.Categories.FirstOrDefault(category => category.Name == creationDto.CategoryName.ToLower())?.Id ?? -1,
        };
        
        if (url.CategoryId == -1)
        {
            throw APIException.CreateException(APIException.Code.CT_01, "Category not found",
                APIException.Type.NOT_FOUND);
        }
        
        try
        {
            _context.Urls.Add(url); 
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

        return url;
    }
    
    public void addClick(int id)
    {
        XYZ? urlToChange = getById(id);
        
        urlToChange.Clicks++;

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

    public void deleteUrl(int id)
    {
        XYZ? urlToDelete = getById(id);

        try
        {
            _context.Urls.Remove(urlToDelete);
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

    public void deleteUrl(string urlShort)
    {
        XYZ? urlToDelete = getUrlLongByShort(urlShort);

        try
        {
            _context.Urls.Remove(urlToDelete);
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