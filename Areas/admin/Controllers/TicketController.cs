using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using E_ticket.Repostoris.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class TicketController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITicketRepository _ticketRepository;
        private readonly ApplicationDbContext _context;

        public TicketController(UserManager<ApplicationUser> userManager, ITicketRepository ticketRepository, ApplicationDbContext context)
        {
           
            _userManager = userManager;
            _ticketRepository = ticketRepository;
            _context = context;
        }


        public async Task<IActionResult> Index(TicketFilterVM ticketFilterVM , int page =1)
        {
            var ticket = await _ticketRepository.GetAsync(includes: [e => e.applicationUser]);
            if (ticketFilterVM.ticketStatus is not null)
            {
                ticket = ticket.Where(e => e.ticketStatus == ticketFilterVM.ticketStatus).ToList();
                ViewBag.status = ticketFilterVM.ticketStatus;
            }
            if (ticketFilterVM.paymenMethod is not null)
            {
                ticket = ticket.Where(e => e.paymenMethod == ticketFilterVM.paymenMethod).ToList();
                ViewBag.payment = ticketFilterVM.paymenMethod;
            }
            if (ticketFilterVM.MaxPrice is not null)
            {
                ticket = ticket.Where(e => e.TotalPrice < ticketFilterVM.MaxPrice).ToList();
                ViewBag.Maxpric = ticketFilterVM.MaxPrice;
            }
            if (ticketFilterVM.MinPrice is not null)
            {
                ticket = ticket.Where(e => e.TotalPrice > ticketFilterVM.MinPrice).ToList();
                ViewBag.minpeice = ticketFilterVM.MinPrice;
            }
            if (ticketFilterVM.FromeDate != null)
            {
                ticket = ticket.Where(e => e.DateTime.Date >= ticketFilterVM.FromeDate.Value.Date);
                ViewBag.fromdate = ticketFilterVM.FromeDate;
            }
            if (ticketFilterVM.ToDate != null)
            {
                ticket = ticket.Where(e => e.DateTime.Date <= ticketFilterVM.ToDate.Value.Date);
                ViewBag.todate = ticketFilterVM.ToDate;
            }
         
            if (ticketFilterVM.UserName is not null)
            {
               
                ticket = ticket.Where(e => e.ApplicationUserId == ticketFilterVM.UserName).ToList();
                ViewBag.userName = ticketFilterVM.UserName;
            }
          


          //pagination
            if (page < 0)
            {
                page = 1;
            }
            var TotallNmberOfPage = Math.Ceiling(ticket.Count() / 10.0);
            ticket = ticket.Skip((page - 1) * 10).Take(10).ToList();
            ViewBag.TotallNumberOfPage = TotallNmberOfPage;
            ViewBag.currentPage = page;

            return View(ticket);
           
        }
        public async Task<IActionResult> Details(int id)
        {
            var ticket =await _ticketRepository.GetOneAsync(e => e.Id == id, includes: [e => e.applicationUser]);
            if (ticket is null)
            {
                return NotFound();
            }
            return View(ticket);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var ticket =await _ticketRepository.GetOneAsync(e => e.Id == id);
            if (ticket is not null)
            {
                _ticketRepository.Delete(ticket);
               await _ticketRepository.CommitAsync();
                TempData["successfull notification"] = "  Delete Ticket Successfull ";
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }
        public async Task<IActionResult> Booked(int id)
        {
            var ticket =await _ticketRepository.GetOneAsync(e => e.Id == id, includes: [e => e.applicationUser]);
            if (ticket is null)
            {
                TempData["success-notification"] = "  ticket not found";
                return RedirectToAction(nameof(Index));
            }
            ticket.ticketStatus = TicketStatus.Booked;
           await _ticketRepository.CommitAsync();
            TempData["successfull notification"] = " Ticket the status Booked Successfull ";
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Pending(int id)
        {
            var ticket = await _ticketRepository.GetOneAsync(e => e.Id == id, includes: [e => e.applicationUser]);
            if (ticket is null)
            {
                return NotFound();
            }
            ticket.ticketStatus = TicketStatus.pending;
            await _ticketRepository.CommitAsync();
            TempData["successfull notification"] = " Ticket the status completed Successfull ";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Canceled(int id)
        {
            var ticket = await _ticketRepository.GetOneAsync(e => e.Id == id, includes: [e => e.applicationUser
            ]);
            if (ticket is null)
            {
                return NotFound();
            }
            ticket.ticketStatus = TicketStatus.canceled;
            await _ticketRepository.CommitAsync();
            TempData["successfull notification"] = " Ticket the status Canceled Successfull ";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewBag.applicationuser = _userManager.Users.Select(e => new SelectListItem()
            {
                Value = e.Id,
                Text = e.UserName
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.applicationuser = _userManager.Users.Select(e => new SelectListItem()
                {
                    Value = e.Id,
                    Text = e.UserName
                }).ToList();
                return View(ticket);
            }
           await _ticketRepository.CreateAsync(ticket);
          await  _ticketRepository.CommitAsync();
            TempData["successfull notification"] = "Successfull Add Ticket ";
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketRepository.GetOneAsync(e => e.Id == id);
            if (ticket is not null)
            {
                ViewBag.applicationuser = _userManager.Users.Select(e => new SelectListItem()
                {
                    Value = e.Id,
                    Text = e.UserName
                }).ToList();
                return View(ticket);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.applicationuser = _userManager.Users.Select(e => new SelectListItem()
                {
                    Value = e.Id,
                    Text = e.UserName
                }).ToList();
                return View(ticket); 
            }
            _ticketRepository.Edit(ticket);
          await  _ticketRepository.CommitAsync();
            TempData["successfull notification"] = "Successfull Update Ticket ";
            return RedirectToAction(nameof(Index));
        }
    }
}
