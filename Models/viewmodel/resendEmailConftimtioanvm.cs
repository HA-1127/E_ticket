using System.ComponentModel.DataAnnotations;

namespace E_ticket.Models.viewmodel
{
    public class resendEmailConftimtioanvm
    {
        public int id { get; set; }
        [Required]
        public string EmailOrName { get; set; } = string.Empty;
    }
}
