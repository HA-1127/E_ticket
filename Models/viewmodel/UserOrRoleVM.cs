namespace E_ticket.Models.viewmodel
{
    public class UserOrRoleVM
    {
        public string? Id { get; set; } 
        public string? FullName { get; set; }
        public string? FristName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public bool islocked { get; set; }
            


    }
}
