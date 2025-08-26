using E_ticket.utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles =$"{SD.Admin},{SD.SuperAdmin}")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
