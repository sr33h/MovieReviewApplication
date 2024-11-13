using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApplicationAPI.Data;
using MovieReviewApplicationAPI.Models;

namespace MovieReviewApplicationAPI.Controllers
{
    [Route("api/MovieApi")]
    [ApiController]
    public class MovieApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MovieApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [EnableCors("Policy1")]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            return Ok(_db.Movies.ToList());
        }

        [EnableCors("Policy1")]
        [HttpGet("{id:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Movie> GetMovie(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var movie = _db.Movies.FirstOrDefault(u => u.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [EnableCors("Policy1")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Movie> CreateMovie([FromBody] Movie movie)
        {
            if (_db.Movies.FirstOrDefault(u => u.MovieName.ToLower() == movie.MovieName.ToLower()) != null)
            {
                ModelState.AddModelError("", "Movie is already added.");
                return BadRequest(ModelState);
            }


            if (movie == null)
            {
                return BadRequest(movie);
            }

            if (movie.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            
            _db.Movies.Add(movie);
            _db.SaveChanges();

            return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);

        }

        [EnableCors("Policy1")]
        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteMovie(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var movie = _db.Movies.FirstOrDefault(u => u.Id == id);

            if (movie == null)
            {
                return NotFound(movie);
            }

            _db.Movies.Remove(movie);
            _db.SaveChanges();


            return NoContent();
        }

        [EnableCors("Policy1")]
        [HttpPut("{id:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (movie == null || id != movie.Id)
            {
                return BadRequest();
            }

            _db.Movies.Update(movie);
            _db.SaveChanges();

            return NoContent();

        }

    }

}
