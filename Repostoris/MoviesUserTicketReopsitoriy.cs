using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class MoviesUserTicketReopsitoriy : Repository<MoviesUserTicket>, IMoviesUserTicketRepositiriy
    {
        public MoviesUserTicketReopsitoriy(ApplicationDbContext context) : base(context)
        {
        }
    }
}
