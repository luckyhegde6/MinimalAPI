using MinimalAPI.Models;

namespace MinimalAPI.Services
{
    public interface IUserServices
    {
        public User Get(UserLogin userLogin);
    }
}
