using E_ticket.Areas.admin.Controllers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
     Booked
    }
    public enum PaymenMethod
    {
        Vias,
        Cash
    }

 
    public class Ticket
    {
       
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser? applicationUser { get; set; }

        public DateTime DateTime { get; set; }
        public string? SessionId { get; set; }
        public string? TransactionId { get; set; }
        public int? Carrier { get; set; }
        public int? CarrierId { get; set; }
        public double TotalPrice { get; set; }
        public TicketStatus ticketStatus { get; set; }
        public PaymenMethod paymenMethod { get; set; }
    }
}
