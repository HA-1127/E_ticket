using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class CineamRepository : Repository<Cinema>, IcinemaRepository
    {
        public CineamRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
