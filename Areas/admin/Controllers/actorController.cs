using E_ticket.data;
using E_ticket.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class actorController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var actor = _context.Actors;
            return View(actor.ToList());
        }
        public IActionResult Create()
        {
            return View(new Actor());
        }
        [HttpPost]
        public IActionResult Create(Actor actor )
        {
            
                _context.Add(actor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "actor");

        }
        
        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor is not null)
            {
                return View(actor);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Actor actor)
        {
            
                _context.Update(actor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "actor");

        }
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor is not null)
            {
                _context.Remove(actor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "actor");

            }
            return NotFound();
        }
          
        


    }
}
