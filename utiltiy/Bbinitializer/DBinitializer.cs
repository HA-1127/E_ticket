using E_ticket.data;
using E_ticket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace E_ticket.utiltiy.Bbinitializer
{
    public class DBinitializer: IDBinitiaitizer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DBinitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

                if (_roleManager.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.Company)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.Employees)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new()
                    {
                        UserName = "SuperAdmin",
                        Email = "hsn768972@gmail.com",
                        FirstName = "Super",
                        LastName = "Admin",
                        EmailConfirmed = true
                    }, "Admin123$").GetAwaiter().GetResult();

                    var user = _userManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();

                    _userManager.AddToRoleAsync(user, SD.SuperAdmin).GetAwaiter().GetResult();
                }
                //
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
