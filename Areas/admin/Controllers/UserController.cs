using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager ,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index(UserOrRoleVM? userOrRoleVM,List<string>roles ,int page = 1)
        {
            var user = _userManager.Users.ToList();
            var UserList = new List<UserOrRoleVM>();
            foreach (var item in user)
            {
              var RoleUser=await  _userManager.GetRolesAsync(item);
                UserList.Add(new UserOrRoleVM
                {
                    Id = item.Id,
                    FullName = item.FirstName + " " + item.LastName,
                    Email = item.Email,
                    UserName = item.UserName,
                    islocked = item.LockoutEnabled,
                    Roles = RoleUser.ToList()
                });
              
            }
            //filters
            if (userOrRoleVM.FristName is not null)
            {
                UserList = UserList.Where(e => e.FristName.Contains(userOrRoleVM.FristName) 
               ).ToList();
                ViewBag.fristName = userOrRoleVM.FristName;
            }
            if (userOrRoleVM.LastName is not null)
            {
                UserList = UserList.Where(e => e.LastName.Contains(userOrRoleVM.LastName)
               ).ToList();
                ViewBag.lastName = userOrRoleVM.LastName;
            }
            if (userOrRoleVM.UserName is not null)
            {
                UserList = UserList.Where(e => e.UserName.Contains(userOrRoleVM.UserName)).ToList();
                ViewBag.username = userOrRoleVM.UserName;
            }
            if (userOrRoleVM.Email is not null)
            {
                UserList = UserList.Where(e => e.Email == userOrRoleVM.Email).ToList();
                ViewBag.email = userOrRoleVM.Email;
            }
            if (userOrRoleVM.islocked)
            {
                UserList = UserList.Where(e => e.islocked == userOrRoleVM.islocked).ToList();
                ViewBag.islocked = userOrRoleVM.islocked;
            }
           
            if (userOrRoleVM is not null)
                {
                    if (roles is not null && roles.Any())
                    {
                        UserList = UserList.Where(e => e.Roles != null && e.Roles.Any(role => roles.Contains(role))).ToList();
                    }

                ViewBag.roles = userOrRoleVM.Roles;
                }
             
            
            ViewBag.roles = _roleManager.Roles.Select(e=>e.Name
          ).ToList();
            //pagination
            if (page < 0)
            {
                page = 1;
            }
            var TotallNmberOfPage = Math.Ceiling(UserList.Count() / 10.0);
            UserList = UserList.Skip((page - 1) * 10).Take(10).ToList();
            ViewBag.totallUnmberofPage = TotallNmberOfPage;
            ViewBag.currentPage = page;
            return View(UserList);
        }
       
        
        public async Task<IActionResult> Details(string id)
        {
            var user =await _userManager.FindByIdAsync(id);
            var RoleUser =await _userManager.GetRolesAsync(user);
            var resualt = new UserOrRoleVM()
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                islocked = user.LockoutEnabled,
                Roles = RoleUser.ToList()

            };
            return View(resualt);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var RoleUser = await _userManager.GetRolesAsync(user);
               await _userManager.DeleteAsync(user);
                TempData["successfull notification"] = "Delete User successfull";

                return RedirectToAction(nameof(Index));

            }
            return NotFound();

        }
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(e => e.Name).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser applicationUser, string Password, List<string> Roles)
        {
            if (ModelState.IsValid)
            {
                var resualt = await _userManager.CreateAsync(applicationUser, Password);
                if (resualt.Succeeded)
                {
                    if (Roles is not null)
                    {
                        foreach (var item in Roles)
                        {
                           await _userManager.AddToRoleAsync(applicationUser, item);
                        }
                        TempData["successfull notification"] = "Add User successfull";

                        return RedirectToAction(nameof(Index));
                    }
                }
                foreach (var item in resualt.Errors)
                {
                    ModelState.AddModelError(" ", item.Description);
                }
            }
            ViewBag.Roles = _roleManager.Roles.Select(e => e.Name).ToList();
           
            return View(applicationUser);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user =await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return View();
            }
           ViewBag.Roleuser =await _userManager.GetRolesAsync(user);
            ViewBag.Role = _roleManager.Roles.Select(e => e.Name).ToList();
            return View(user);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser, List<string> Roles)
        {
            var user =await _userManager.FindByIdAsync(applicationUser.Id);
            if (user is null)
            {
                return NotFound();
            }
          
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.Email = applicationUser.Email;
            user.UserName = applicationUser.UserName;
            user.LockoutEnabled = applicationUser.LockoutEnabled;
             
            var resualt = await _userManager.UpdateAsync(user);
            if (resualt.Succeeded)
            {
                //delet old roles user 
                var RoleUser = await  _userManager.GetRolesAsync(user);
                foreach (var item in RoleUser)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
                // save roles
                if (Roles is not null)
                {
                    foreach (var item in Roles)
                    {
                        await _userManager.AddToRoleAsync(user, item);
                    }
                    TempData["successfull notification"] = "Update User successfull";

                    return RedirectToAction(nameof(Index));
                }
            }
            foreach (var item in resualt.Errors)
            {
                ModelState.AddModelError(" ", item.Description);
            }
            ViewBag.Role = _roleManager.Roles.Select(e => e.Name).ToList();
            return View(applicationUser);

        }
        public async Task<IActionResult> LockedUNlocked(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                if (user.LockoutEnabled)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEnd = null;
                    TempData["successfull notification"] = " User the unlocked ";

                }
                else
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.AddDays(2);
                    TempData["successfull notification"] = $" User locked{user.LockoutEnd}successfull";
                }
               await _userManager.UpdateAsync(user);
        
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
