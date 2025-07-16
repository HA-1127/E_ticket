using System.ComponentModel.DataAnnotations;

namespace E_ticket.Models.viewmodel
{
    public class Loginvm
    {
        public int Id { get; set; }
        [Required]
        public string NameOrEmail { get; set; } = string.Empty;
        [Required,DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool Remmeberme { get; set; }
    }
}
