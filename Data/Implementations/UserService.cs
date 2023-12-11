using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Interfaces;
using url_shortener.Entities;
using url_shortener.Models;

namespace url_shortener.Data.Implementations;

public class UserService : IUserService
{
    private readonly UrlShortenerContext _context;

    public UserService(UrlShortenerContext context)
    {
        _context = context;
    }

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User? GetUser(int id)
    {
        User? user = _context.Users.FirstOrDefault((users) => users.Id == id);

        return user;
    }

    public User? GetUser(string email)
    {
        User? user = _context.Users.FirstOrDefault((users) => users.Email == email);

        return user;
    }

    public void AddUser(UserForCreationDTO userForCreationDto)
    {
        User? userExist = _context.Users.FirstOrDefault((users) => users.Email == userForCreationDto.Email);

        if (userExist != null)
        {
            throw new Exception("NT - User email already exists");
        }

        User user = new()
        {
            Username = userForCreationDto.UserName,
            FirstName = userForCreationDto.FirstName,
            LastName = userForCreationDto.LastName,
            Email = userForCreationDto.Email,
            LimitUrl = userForCreationDto.LimitUrl
        };

        try
        {
            _context.Users.Add(user);
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

    public List<XYZ> GetUrls(int userId)
    {
        User? user = GetUser(userId);
        List<XYZ> urls = _context.Urls.Where(url => url.UserId == user.Id).ToList();
        return urls;
    }
    
    public int GetUserLimitUrl(int userId)
    {
        try
        {
            User? user = _context.Users.FirstOrDefault((users) => users.Id == userId);
            if (user != null)
            {
                var limitUrl = user.LimitUrl;
                return limitUrl;
            }
            else
            {
                throw new Exception("BD - User not found");
            }
        }
        catch (Exception e)
        {
            throw new Exception("IE - An error occurred while getting the data in the database");
        }
    }

    public void UpdateUser(UserForUpdateDTO userForUpdateDto)
    {
        User? toChange = GetUser(userForUpdateDto.UserToChangeID);

        User? userExist = _context.Users.FirstOrDefault((users) => users.Email == userForUpdateDto.Email);

        if (userExist != null)
        {
            throw new Exception("NT - User email already exists");
        }

        toChange.FirstName = userForUpdateDto.FirstName;
        toChange.LastName = userForUpdateDto.LastName;
        toChange.Email = userForUpdateDto.Email;

        try
        {
            _context.Users.Update(toChange);
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

    public void DeleteUser(int userId)
    {
        User? toRemove = GetUser(userId);

        try
        {
            _context.Users.Remove(toRemove);
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
    
    public void ResetUserLimitUrl(int userId)
    {
        User? toChange = GetUser(userId);

        if (toChange != null)
        {
            toChange.LimitUrl = 10;

            try
            {
                _context.Users.Update(toChange);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new Exception("IE - An error occurred while setting the data in the database");
            }
        }
    }
}