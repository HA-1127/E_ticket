using System.ComponentModel.DataAnnotations;

namespace E_ticket.Models
{
    public class UserOtp
    {
        public int id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ExprationDate { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser applicationUser { get; set; } = null!;
    }
}
