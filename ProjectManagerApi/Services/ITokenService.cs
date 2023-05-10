using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}