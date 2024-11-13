using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApplicationAPI.Data;
using MovieReviewApplicationAPI.Models;
using MovieReviewApplicationAPI.Models.Dto;

namespace MovieReviewApplicationAPI.Controllers
{
    [Route("api/ReviewApi")]
    [ApiController]
    public class ReviewApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ReviewApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [EnableCors("Policy1")]
        [HttpGet]
        public ActionResult<IEnumerable<Review>> GetAllReviews()
        {
            return Ok(_db.Reviews.ToList());
        }


        [EnableCors("Policy1")]
        [HttpGet("{movieid:int}", Name = "GetReviewsForMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Review>> GetReviewsForMovie(int movieid)
        {
            if (movieid == 0)
            {
                return BadRequest();
            }
            var reviews = _db.Reviews.ToList().FindAll(u => u.MovieId == movieid);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }


        [EnableCors("Policy1")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Review> CreateReview([FromBody] ReviewDto reviewdto)
        {
            if (reviewdto == null)
            {
                return BadRequest(reviewdto);
            }

            if (reviewdto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Review review = new Review
            {
                ReviewComment = reviewdto.ReviewComment,
                MovieId = reviewdto.MovieId,
                movie = _db.Movies.ToList().Find(u => u.Id == reviewdto.MovieId)
            };

            _db.Reviews.Add(review);
            _db.SaveChanges();

            return CreatedAtRoute("GetReviewsForMovie", new { movieid = review.MovieId }, review);

        }



        [EnableCors("Policy1")]
        [HttpPut("{id:int}", Name = "UpdateReview")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateReview(int id, [FromBody] ReviewUpdateDto reviewupdatedto)
        {
            if (reviewupdatedto == null || id != reviewupdatedto.Id)
            {
                return BadRequest();
            }

            if(_db.Reviews.FirstOrDefault(u=>u.Id == id) == null)
            {
                return BadRequest();
            }

            Review review = _db.Reviews.FirstOrDefault(u => u.Id == id);

            review.ReviewComment = reviewupdatedto.ReviewComment;

            _db.Reviews.Update(review);
            _db.SaveChanges();

            return NoContent();
        }

        [EnableCors("Policy1")]
        [HttpDelete("{id:int}", Name = "DeleteReview")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteReview(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var review = _db.Reviews.FirstOrDefault(u => u.Id == id);

            if (review == null)
            {
                return NotFound(review);
            }

            _db.Reviews.Remove(review);
            _db.SaveChanges();

            return NoContent();
        }

    }
    
}
