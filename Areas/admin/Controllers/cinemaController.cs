using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using E_ticket.utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles =$"{SD.SuperAdmin},{SD.Admin}")]
    public class cinemaController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index(CinemaFilterVM? cinemaFilterVM ,int page =1)
        {
            var cinema = _context.Cinemas.Include(e => e.Movies).ToList();
            //filters
            if (cinemaFilterVM.CinemaName is not null)
            {
                cinema = cinema.Where(e => e.Name.Contains(cinemaFilterVM.CinemaName)).ToList();
                ViewBag.cinemaName = cinemaFilterVM.CinemaName;
            }
            if (cinemaFilterVM.Address is not null)
            {
                cinema = cinema.Where(e => e.Address == cinemaFilterVM.Address).ToList();
                ViewBag.address = cinemaFilterVM.Address;
            }
            //pagination
            if (page < 0)
            {
                page = 1;
            }
            var TotallNumberOfPage = Math.Ceiling(cinema.Count() / 10.0);
            cinema = cinema.Skip((page - 1) * 10).Take(10).ToList();
            ViewBag.TotallNumberOfPage = TotallNumberOfPage;
            ViewBag.CurrentPage = page;
            return View(cinema);
        }
        public IActionResult Create()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            if(!ModelState.IsValid)
            {
                return View(cinema);
            }
            _context.Add(cinema);
            TempData["successfull notification"] = "succsefull add cinema";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), "cinema");
        }
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);
            if (cinema is not null)
            {

                return View(cinema);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            
                _context.Update(cinema);
            TempData["successfull notification"] = "successfull edit cinema";
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "cinema");
           
        }
        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);
            if (cinema is not null)
            {
                _context.Remove(cinema);
                TempData["successfull notification"] = "succssefull delete cinema";
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "cinema");
            }
            return NotFound();
        }

    }
}
