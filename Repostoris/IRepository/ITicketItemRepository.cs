using E_ticket.Models;

namespace E_ticket.Repostoris.IRepository
{
    public interface ITicketItemRepository :IRepository<Ticket>
    {
         Task CreatRangeAsyac(List<TicketItme> ticketItmes);
    }
}
