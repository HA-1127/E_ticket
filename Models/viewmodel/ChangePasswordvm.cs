namespace E_ticket.Models.viewmodel
{
    public class ChangePasswordvm
    {
        public int id { get; set; }

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Userid { get; set; }
    }
}
