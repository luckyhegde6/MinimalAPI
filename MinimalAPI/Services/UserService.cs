using MinimalAPI.Models;
using MinimalAPI.Repository;

namespace MinimalAPI.Services
{
    public class UserService : IUserServices
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepo.Users.FirstOrDefault(o => o.UserName.Equals
            (userLogin.UserName, StringComparison.OrdinalIgnoreCase)
               && o.Password.Equals(userLogin.Password));

            return user;
        }
    }
}
