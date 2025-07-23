using Microsoft.EntityFrameworkCore;

namespace E_ticket.Models
{
    public enum TicketStatus
    {
     pending,
     processing,
     shipped,
     completed,
     canceled,
     refunded,
    }
    public enum PaymenMethod
    {
        Vias,
        Cash
    }
    [PrimaryKey(nameof(IdApplicationUser))]
    public class Ticket
    {
        public int Id { get; set; }
        public int IdApplicationUser { get; set; }
      public ApplicationUser applicationuser { get; set; }
        public DateTime DateTime { get; set; }
        public int? SessionId { get; set; }
        public int? TransactionId { get; set; }
        public int? Carrier { get; set; }
        public int? CarrierId { get; set; }
        public double TotalPrice { get; set; }
        public TicketStatus ticketStatus { get; set; }
        public PaymenMethod paymenMethod { get; set; }
    }
}
