using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class RuserotpRepository : Repository<UserOtp>, IRuserotprepostoity
    {
        public RuserotpRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
