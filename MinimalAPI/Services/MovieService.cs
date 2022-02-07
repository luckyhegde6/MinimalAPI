using MinimalAPI.Models;
using MinimalAPI.Repository;

namespace MinimalAPI.Services
{
    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {
            movie.Id = MovieRepo.Movies.Count + 1;
            MovieRepo.Movies.Add(movie);
            return movie;
        }

        public List<Movie> List()
        {
            var movies = MovieRepo.Movies;
            return movies;
        }

        public Movie Update(Movie newMovie)
        {
            var oldMovie = MovieRepo.Movies.FirstOrDefault(o => o.Id == newMovie.Id);
            if (oldMovie == null) return null;
            oldMovie.Title = newMovie.Title;
            oldMovie.Description = newMovie.Description;
            oldMovie.Rating = newMovie.Rating;

            return oldMovie;
        }

        public bool Delete(int Id)
        {
            var oldMovie = MovieRepo.Movies.FirstOrDefault(o => o.Id == Id);
            if (oldMovie == null) return false;

            MovieRepo.Movies.Remove(oldMovie);
            return true;
        }

        public Movie Get(int Id)
        {
            var movie = MovieRepo.Movies.FirstOrDefault(o => o.Id == Id);
            if (movie == null) return null;
            return movie;
        }
    }
}
