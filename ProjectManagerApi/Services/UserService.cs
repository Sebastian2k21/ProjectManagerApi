using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Extensions;
using System.Security.Cryptography;

namespace ProjectManagerApi.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService tokenService;
        private readonly IBaseRepository<User> userRepository;
        private readonly IBaseRepository<Tech> techRepository;
        private readonly IBaseRepository<Language> languageRepository;

        public UserService(ITokenService tokenService, IBaseRepository<User> userRepository, IBaseRepository<Tech> techRepository, IBaseRepository<Language> languageRepository)
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.techRepository = techRepository;
            this.languageRepository = languageRepository;
        }

        public async Task<User> AddUser(User user, string password, List<int> technologies, List<int> languages)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;
            user.Technologies.AddRange(await techRepository.GetCollectionFromDB(technologies, "Invalid technology ID"));
            user.Laguages.AddRange(await languageRepository.GetCollectionFromDB(languages, "Invalid language ID"));
            await userRepository.Add(user);
            return user;
        }

        public async Task<string> Auth(string email, string password)
        {
            var user = await userRepository.FindFirst(x => x.Email == email);
            if (user == null || !ComparePassword(user, password))
            {
                throw new InvalidCredentialsException();
            }
            return  tokenService.CreateToken(user);
        }

        private bool ComparePassword(User user, string password)
        {
            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return user.PasswordHash.SequenceEqual(hash); //porownuje czy w obu tabliacach sa takie same elementy, bez tego trzeba by bylo uzyc petli
        }
    }
}
