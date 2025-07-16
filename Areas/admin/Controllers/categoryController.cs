using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class categoryController : Controller
    {
        ApplicationDbContext _context = new();
        private readonly ICategotyRepository _categotyRepository;

        public categoryController(ICategotyRepository categotyRepository)
        {
            _categotyRepository = categotyRepository;
        }
        public IActionResult Index()
        {
            var category = _context.Categories;
            return View(category.ToList());
        }
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _context.Add(category);
            TempData["successfull notification"] = "succsefull add category";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), "category", new { area = "admin" });
        }
        public IActionResult Edit(int id)
        {
            var categery = _context.Categories.FirstOrDefault(e => e.Id == id);
            if (categery is not null)
            {
                return View(categery);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _context.Update(category);
            TempData["successfull notification"] = "succsefull edit category";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), "category", new { area = "admin" });

        }
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(e => e.Id == id);
            if (category is not null)
            {
                _context.Remove(category);
                TempData["successfull notification"] = "succsefull delete category";
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "category", new { area = "admin" });
            }
            return NotFound();
        }
            
    }
}
