using url_shortener.Data.Interfaces;
using url_shortener.Entities;
using url_shortener.Models;
using url_shortener.Utilities;

namespace url_shortener.Data.Implementations;

public class XYZService : IXYZService
{
    private readonly UrlShortenerContext _context;

    public XYZService(UrlShortenerContext context)
    {
        _context = context;
    }

    public List<XYZ> GetAll()
    {
        return _context.Urls.ToList();
    }

    public XYZ? getById(int id)
    {
        var url = _context.Urls.FirstOrDefault(url => url.Id == id);

        return url;
    }

    public XYZ? getUrlLongByShort(string urlShort)
    {
        var url = _context.Urls.FirstOrDefault(url => url.UrlShort == urlShort);

        return url;
    }

    public bool isUrlShortExist(string urlShort)
    {
        if (string.IsNullOrWhiteSpace(urlShort))
        {
            throw new Exception("BD - Url short is required");
        }

        return _context.Urls.Any(url => url.UrlShort == urlShort);
    }

    public bool isUrlLongExist(string urlLong)
    {
        if (string.IsNullOrWhiteSpace(urlLong))
        {
            throw new Exception("BD - Url long is required");
        }

        return _context.Urls.Any(url => url.UrlLong == urlLong);
    }

    public XYZ createUrl(XYZForCreationDTO creationDto)
    {
        string randomUrl = UrlMaker.RandomString(6);

        while (isUrlShortExist(randomUrl))
        {
            randomUrl = UrlMaker.RandomString(6);
        }

        if (!Uri.IsWellFormedUriString(creationDto.UrlLong, UriKind.Absolute))
        {
            throw new Exception("BD - Url long is not valid");
        }

        var url = new XYZ
        {
            Name = creationDto.Name,
            UrlLong = creationDto.UrlLong,
            UrlShort = randomUrl,
            UserId = creationDto.UserId,
            CategoryId = _context.Categories
                .FirstOrDefault(category => category.Name == creationDto.CategoryName.ToLower())?.Id ?? -1,
        };

        try
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == creationDto.UserId);
            if (user != null)
            {
                if (user.LimitUrl > 0)
                {
                    _context.Urls.Add(url);
                    user.LimitUrl--;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("BD - User has no more urls to create");
                }
            }
            else
            {
                throw new Exception("BD - User not found");
            }
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

        return url;
    }

    public void addClick(int id)
    {
        XYZ? urlToChange = getById(id);

        if (urlToChange == null)
        {
            throw new Exception("BD - Url not found");
        }

        urlToChange.Clicks++;

        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }

    public void deleteUrl(int id)
    {
        XYZ? urlToDelete = getById(id);

        if (urlToDelete == null)
        {
            throw new Exception("BD - Url not found");
        }

        try
        {
            _context.Urls.Remove(urlToDelete);
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

    public void deleteUrl(string urlShort)
    {
        XYZ? urlToDelete = getUrlLongByShort(urlShort);

        if (urlToDelete == null)
        {
            throw new Exception("BD - Url not found");
        }

        try
        {
            _context.Urls.Remove(urlToDelete);
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