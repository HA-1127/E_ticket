using Microsoft.EntityFrameworkCore;

namespace E_ticket.Models
{
    [PrimaryKey(nameof(TicketId), nameof(MovieId))]
    public class TicketItme
    {
        public int TicketId  { get; set; }

        public Ticket ticket { get; set; }
        public int MovieId  { get; set; }
        public Movie movie { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
