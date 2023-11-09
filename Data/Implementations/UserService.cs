using url_shortener.Entities;
using url_shortener.Models;
using url_shortener.Models.Repository.Interface;
using url_shortener.Utilities;

namespace url_shortener.Data.Implementations;
public class UserService : IUserService
{
    private readonly UrlShortenerContext _context;
    
    public UserService (UrlShortenerContext context)
    {
        _context = context;
    }
    
    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUser(int id)
    {
        User? user = _context.Users.FirstOrDefault((users) => users.Id == id);

        if (user == null)
        {
           throw APIException.CreateException(
                           APIException.Code.US_01,
                           "User not found",
                           APIException.Type.NOT_FOUND);
        }

        return user;
    }

    public User GetUser(string email)
    {
        User? user = _context.Users.FirstOrDefault((users) => users.Email == email);
        if (user == null)
        {
            throw APIException.CreateException(
                APIException.Code.US_01,
                "User not found",
                APIException.Type.NOT_FOUND);
        }

        return user;
    }
    
    public void AddUser(UserForCreationDTO userForCreationDto)
    {
        User? userExist = _context.Users.FirstOrDefault((users) => users.Email == userForCreationDto.Email);
        if (userExist != null)
        {
            throw APIException.CreateException(
                APIException.Code.US_02,
                "User email already exists",
                APIException.Type.BAD_REQUEST);
        }
        
        User user = new()
        {
            Username = userForCreationDto.UserName,
            FirstName = userForCreationDto.FirstName,
            LastName = userForCreationDto.LastName,
            Email = userForCreationDto.Email,
        };
        
        try
        {
            _context.Users.Add(user);
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_01,
                "An error occurred while setting the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
        
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_02,
                "An error occurred while saving the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }

    public List<XYZ> GetUrls(int userId)
    {
        User? user = GetUser(userId);
        List<XYZ> urls = _context.Urls.Where(url => url.UserId == user.Id).ToList();
        return urls;
    }


    public void UpdateUser(UserForUpdateDTO userForUpdateDto)
    {
        User? toChange = GetUser(userForUpdateDto.UserToChangeID);
        
        User? userExist = _context.Users.FirstOrDefault((users) => users.Email == userForUpdateDto.Email);
        if (userExist != null)
        {
            throw APIException.CreateException(
                APIException.Code.US_02,
                "User email already exists",
                APIException.Type.BAD_REQUEST);
        }
        
        toChange.FirstName = userForUpdateDto.FirstName;
        toChange.LastName = userForUpdateDto.LastName;
        toChange.Email = userForUpdateDto.Email;

        try
        { 
            _context.Users.Update(toChange);
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_01,
                "An error occurred while setting the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
        
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_02,
                "An error occurred while saving the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }

    public void DeleteUser(int userId)
    {
        User? toRemove = GetUser(userId);

        try
        { 

            _context.Users.Remove(toRemove);
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_01,
                "An error occurred while setting the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
        
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw APIException.CreateException(
                APIException.Code.DB_02,
                "An error occurred while saving the data in the database",
                APIException.Type.INTERNAL_SERVER_ERROR);
        }
    }
}