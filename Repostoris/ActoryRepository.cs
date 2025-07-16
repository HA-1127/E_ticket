using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class ActoryRepository : Repository<Actor>, Iactoryrepository
    {
        public ActoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
