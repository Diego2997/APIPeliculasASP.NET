using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _pelrepo;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository pelrepo, IMapper mapper)
        {
            _pelrepo = pelrepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMovies()
        {
            var listMovies = _pelrepo.GetMovies();
            var listMoviesDto = new List<MovieDto>();

            foreach (var movie in listMovies)
            {
                listMoviesDto.Add(_mapper.Map<MovieDto>(movie));
            }
            return Ok(listMoviesDto);
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetMovie))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int id)
        {
            var movie = _pelrepo.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            var itemMovieDto = _mapper.Map<MovieDto>(movie);
            return Ok(itemMovieDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] MovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (createMovieDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelrepo.ExistMovie(createMovieDto.Name))
            {
                ModelState.AddModelError("", "Movie exist");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);
            if (!_pelrepo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"something went wrong saving the record {movie.Name}");
                return StatusCode(500, ModelState);
            }

            //return CreatedAtAction(nameof(GetMovie), new { categoryId = movie.Id });
            return CreatedAtRoute("GetMovie", new { movieId = movie.Id });
        }

        [HttpPatch("{id:int}", Name = "UpdatePatchMovie")]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchMovie(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            var movie = _mapper.Map<Movie>(movieDto);
            if (!_pelrepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"something went wrong updating the registry {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMovie(int id)
        {
            if (!_pelrepo.ExistMovie(id))
            {
                return NotFound();
            }


            var movie = _pelrepo.GetMovie(id);
            if (!_pelrepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"something went wrong delete the registry {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



        [HttpGet("MovieInCategory/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult MovieInCategory(int id)
        {
            var listMovies = _pelrepo.GetMoviesInCategory(id);
            if(listMovies == null)
            {
                return NotFound();
            }
            var itemMovie = new List<MovieDto>();

            foreach (var movie in listMovies)
            {
                itemMovie.Add(_mapper.Map<MovieDto>(movie));
            }
            return Ok(itemMovie);
        }

        [HttpGet("SearcMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult SearcMovie(string name)
        {
            try
            {
                var search = _pelrepo.SearchMovie(name.Trim());
                if(search.Any())
                {
                    return Ok(search);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
         
        }
    }
}
