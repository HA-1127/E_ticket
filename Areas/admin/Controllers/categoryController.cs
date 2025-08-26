using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using E_ticket.Repostoris.IRepository;
using E_ticket.utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles =$"{SD.SuperAdmin},{SD.Admin}")]
    public class categoryController : Controller
    {
        ApplicationDbContext _context = new();
        private readonly ICategotyRepository _categotyRepository;

        public categoryController(ICategotyRepository categotyRepository)
        {
            _categotyRepository = categotyRepository;
        }
        public IActionResult Index(CatogerysFiltersVM? catogerysFiltersVM , int page =1)
        {
            var category = _context.Categories.ToList();
            if (catogerysFiltersVM.CatogeryName is not null)
            {
                category = category.Where(e=>e.Id ==catogerysFiltersVM.CatogeryName).ToList();
                ViewBag.catogeryName = catogerysFiltersVM.CatogeryName;

            }
            //pagination
            if (page < 0)
            {
                page = 1;
            }
            var TotallNumberOfPage = Math.Ceiling(category.Count() / 10.0);
            category = category.Skip((page - 1) * 10).Take(10).ToList();
            ViewBag.TotallNumberOfPage = TotallNumberOfPage;
            ViewBag.CurrentPage = page;
            return View(category);
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
