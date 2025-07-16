using System.ComponentModel.DataAnnotations;

namespace E_ticket.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MinLength (3)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool Status { get; set; }
        public ICollection<MovieImage> Images { get; set; } = new List<MovieImage>();
        public DateTime StartDate { get; set; }
        [Display(Name = "Cinema")]
        public int CinemaId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Cinema Cinema { get; set; }
        public Category Category { get; set; }

        public ICollection<ActorMovie> actorsMovies { get; set; } = new List<ActorMovie>();
    }
}
