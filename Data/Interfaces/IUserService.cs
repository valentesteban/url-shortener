using url_shortener.Entities;

namespace url_shortener.Models.Repository.Interface;

public interface IUserService
{
    public List<User> GetUsers();
    public User? GetUser(int id);
    public User? GetUser(String email);
    public void AddUser(UserForCreationDTO userForCreationDto);
    public List<XYZ> GetUrls(int userId);
    public void UpdateUser(UserForUpdateDTO userForUpdateDto);
    public void DeleteUser(int userId);
}