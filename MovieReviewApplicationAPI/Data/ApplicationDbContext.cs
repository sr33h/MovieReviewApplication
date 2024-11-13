using Microsoft.EntityFrameworkCore;
using MovieReviewApplicationAPI.Models;

namespace MovieReviewApplicationAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Movie> Movies {  get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(
                new Movie {Id=1, MovieName = "Wild Robot"},
                new Movie {Id=2, MovieName = "Inside Out 2"},
                new Movie {Id=3, MovieName = "Iron Giant"});

            modelBuilder.Entity<Review>().HasData(
              new Review { Id = 1, ReviewComment = "Excellent animation", MovieId=1},
              new Review { Id = 2, ReviewComment = "Excellent concept", MovieId = 2 },
              new Review { Id = 3, ReviewComment = "Excellent story", MovieId = 3 },
              new Review { Id = 4, ReviewComment = "Heart warming", MovieId = 1 },
              new Review { Id = 5, ReviewComment = "Emotional", MovieId = 2 },
              new Review { Id = 6, ReviewComment = "Made me cry", MovieId = 3 }
                );
        }
    }
}
