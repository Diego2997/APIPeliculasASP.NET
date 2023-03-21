using APIPeliculas.Models;

namespace APIPeliculas.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int id);
        bool ExistMovie(string name);
        bool ExistMovie(int id);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);
        ICollection<Movie> GetMoviesInCategory(int catId);
        ICollection<Movie> SearchMovie(string name);
        bool Save();
    }
}
