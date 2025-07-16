using E_ticket.Areas.admin.Controllers;
using E_ticket.data;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class Movierepository : Repository<movie>, Imovierepository
    {
        public Movierepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
