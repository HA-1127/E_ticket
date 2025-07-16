using Microsoft.AspNetCore.Identity;
using System.Runtime.ExceptionServices;

namespace E_ticket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ? Address  {get; set;}

    }
}
