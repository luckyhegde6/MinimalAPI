using MinimalAPI.Models;

namespace MinimalAPI.Repository
{
    public class UserRepo
    {
        public static List<User> Users = new()
        {
            new()
            {
                UserName = "Lucky_admin",
                Password = "Admin@123",
                Email = "luckyhegde@yahoo.in",
                FirstName = "Lucky",
                LastName = "Hegde",
                Role = "admin",
                UserId = 1
            },
            new()
            {
                UserName = "Lucky",
                Password = "Lucky@123",
                Email = "luckyhegde@yahoo.co.in",
                FirstName = "Lucky",
                LastName = "Hegde",
                Role = "user",
                UserId = 2
            },
            new()
            {
                UserName = "James",
                Password = "James@123",
                Email = "james@yahoo.co.in",
                FirstName = "James",
                LastName = "Nobody",
                Role = "user",
                UserId = 3
            }
        };
    }
}
