using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;
using System.Threading.Tasks;

namespace E_ticket.Repostoris
{
    public class TicketItemRepository : Repository<Ticket>, ITicketItemRepository
    {
        public TicketItemRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task CreatRangeAsyac(List<TicketItme> ticketItmes)
        {
          await  Context.ticketItmes.AddRangeAsync(ticketItmes);
        }
    }
}
