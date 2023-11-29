using url_shortener.Entities;
using url_shortener.Models;

namespace url_shortener.Data.Interfaces;

public interface IUserService
{
    public List<User> GetUsers();
    public User? GetUser(int id);
    public User? GetUser(String email);
    public void AddUser(UserForCreationDTO userForCreationDto);
    public List<XYZ> GetUrls(int userId);
    public void GetUserLimitUrl(int userId);
    public void UpdateUser(UserForUpdateDTO userForUpdateDto);
    public void DeleteUser(int userId);
    public void ResetUserLimitUrl(int userId);
}