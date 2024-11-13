using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewApplicationAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ReviewComment { get; set; }

        [Required]
        [ForeignKey("movie")]
        public int MovieId {  get; set; }

        public Movie movie { get; set; }
    }
}
