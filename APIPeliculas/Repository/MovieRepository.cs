﻿using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repository.IRepository;

namespace APIPeliculas.Repository
{
    public class MovieRepository : IMovieRepository
    {

        private readonly APIContext _context;
        public MovieRepository(APIContext context)
        {
            _context = context;
        }
        public bool CreateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _context.Movies.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
            return Save();
        }

        public bool ExistMovie(string name)
        {
            bool value = _context.Movies.Any(
                c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ExistMovie(int id)
        {
            return _context.Movies.Any(c => c.Id == id);
        }

        public ICollection<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(c => c.Name).ToList();
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _context.Movies.Update(movie);
            return Save();
        }

        public ICollection<Movie> GetMoviesInCategory(int id)
        {
            throw new NotImplementedException();
        }
        public ICollection<Movie> SearchMovie(string name)
        {
            throw new NotImplementedException();
        }
    }
}
