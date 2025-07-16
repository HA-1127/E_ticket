using E_ticket.data;
using E_ticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class cinemaController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var cinema = _context.Cinemas.Include(e => e.Movies);
            return View(cinema.ToList());
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
