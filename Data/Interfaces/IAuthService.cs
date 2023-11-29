using url_shortener.Entities;
using url_shortener.Models;

namespace url_shortener.Data.Interfaces;

public interface IAuthService
{
    public Auth Authenticate(UserForLoginDTO userForLoginDto);
    public string GenerateToken(Auth auth);
    public Auth getCurrentUser();
    public Boolean isSameUserRequest(int userId);
}