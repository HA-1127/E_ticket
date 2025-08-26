using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using E_ticket.utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ticket.Areas.admin.Controllers
{

    [Area("admin")]
    [Authorize(Roles =$"{SD.SuperAdmin},{SD.Admin}")]
    public class actorController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index(ActorFiltersVM? actorFiltersVM , int page = 1)
        {
            var actor = _context.Actors.ToList();
            //filters
            if (actorFiltersVM.FirstName is not null)
            {
                actor = actor.Where(e => e.FirstName.Contains(actorFiltersVM.FirstName)).ToList();
                ViewBag.actorfristName = actorFiltersVM.FirstName;
            }
            if (actorFiltersVM.LastName is not null)
            {
                actor = actor.Where(e => e.LastName.Contains(actorFiltersVM.LastName)).ToList();
                ViewBag.actorlastName = actorFiltersVM.LastName;
            }
            //pagination
            if (page < 0)
            {
                page = 1;
            }
            var TotallNumberOfPage = Math.Ceiling(actor.Count() / 10.0);
            actor = actor.Skip((page - 1) * 10).Take(10).ToList();
            ViewBag.totallnmberofpage = TotallNumberOfPage;
            ViewBag.CurrentPage = page;
            return View(actor);
        }
        public IActionResult Create()
        {
            return View(new Actor());
        }
        [HttpPost]
        public IActionResult Create(Actor actor ,IFormFile Image )
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            if (Image is not null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);
                //save image in wwwroot
                using (var streem = System.IO.File.Create(filePath))
                {
                    Image.CopyTo(streem);
                }
                //save in db
                actor.ProfilePicture = filePath;

                _context.Add(actor);
                _context.SaveChanges();
                TempData["successfull notification"] = "successfull add actore";

                return RedirectToAction(nameof(Index), "actor");

            }

            return BadRequest();
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
        public IActionResult Edit(Actor actor ,IFormFile? Image)
        {
            var actorInDb = _context.Actors.AsNoTracking().FirstOrDefault(e => e.Id == actor.Id);
            if (actorInDb is not null)
            {
                if (!ModelState.IsValid)
                {
                    return View(actor);
                }
                if (Image is not null && Image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);
                    //save image in wwwroot
                    using (var streem = System.IO.File.Create(filePath))
                    {
                        Image.CopyTo(streem);
                    }
                    //delet image old in wwwroot
                    var OldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actorInDb.ProfilePicture);
                    if (System.IO.File.Exists(OldFilePath))
                    {
                        System.IO.File.Delete(OldFilePath);
                    }
                    //save  image in db
                    actor.ProfilePicture = filePath;
                }
                else
                {
                    actor.ProfilePicture = actorInDb.ProfilePicture;
                }
                _context.Update(actor);
                _context.SaveChanges();
                TempData["successfull notification"] = " successfull update actore";

                return RedirectToAction(nameof(Index), "actor");

            }

            return NotFound();
        }
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (actor is not null)
            {
                //delet in wwwroot
               var OldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actor.ProfilePicture);
                if (System.IO.File.Exists(OldFilePath))
                {
                    System.IO.File.Delete(OldFilePath);
                }
                _context.Remove(actor);
                _context.SaveChanges();
                TempData["successfull notification"] = " successfull delete actore";

                return RedirectToAction(nameof(Index), "actor");

            }
            return NotFound();
        }
          
        


    }
}
