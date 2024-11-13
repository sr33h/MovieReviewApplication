using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieReviewApplicationAPI.Models.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }      
        public string ReviewComment { get; set; }       
        public int MovieId { get; set; }
    }
}
