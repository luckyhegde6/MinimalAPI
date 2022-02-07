using MinimalAPI.Models;

namespace MinimalAPI.Repository
{
    public class MovieRepo
    {
        public static List<Movie> Movies = new()
        {
            new() { Id = 1, Title = "Eternals", Description = "This is Eternal Movie", Rating = 5.5 },
            new() { Id = 2, Title = "Iron Man", Description = "This is Iron Man Movie", Rating = 8.65 },
            new() { Id = 3, Title = "ZERO", Description = "This is Zero Movie", Rating = 1.24 },
            new() { Id = 4, Title = "SHREK", Description = "This is Shrek Movie", Rating = 8.4 },
            new() { Id = 5, Title = "Titanic", Description = "This is Titanic Movie", Rating = 9.2 }
        };
    }
}
