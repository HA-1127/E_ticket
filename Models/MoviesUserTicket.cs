using Microsoft.EntityFrameworkCore;

namespace E_ticket.Models
{
    [PrimaryKey(nameof(MovieId),nameof(ApplicationUserId))]
    public class MoviesUserTicket
    {
        public int  MovieId  { get; set; }
        public Movie movie { get; set; }
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser applicationUser { get; set; } = null!;
        public int  Count { get; set; }

    }
}
