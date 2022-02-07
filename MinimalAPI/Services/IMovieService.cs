using MinimalAPI.Models;

namespace MinimalAPI.Services
{
    public interface IMovieService
    {
        public Movie Create(Movie movie);
        public Movie Update(Movie movie);
        public bool Delete(int id);
        public  Movie Get(int id);
        public List<Movie> List();
    }
}
