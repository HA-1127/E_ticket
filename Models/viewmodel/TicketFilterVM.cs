namespace E_ticket.Models.viewmodel
{
    public class TicketFilterVM
    { 
        public TicketStatus ? ticketStatus { get; set; }
        public PaymenMethod? paymenMethod { get; set; }
        public string? UserName { get; set; }
        public double? MaxPrice { get; set; }
        public double? MinPrice { get; set; }
        public DateTime? FromeDate { get; set; }
        public DateTime? ToDate { get; set; }
     
    }
}
