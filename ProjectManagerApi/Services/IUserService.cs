using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Services
{
    public interface IUserService
    {
        Task<User> AddUser(User user, string password, List<int> technologies, List<int> languages);
        Task<string> Auth(string email, string password);
    }
}