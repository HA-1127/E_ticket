using E_ticket.validation;
using System.ComponentModel.DataAnnotations;

namespace E_ticket.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
       // [customlengthvalidationattribute(22)]
        public string? Description { get; set; } = string.Empty;

        public string? CinemaLogo { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
