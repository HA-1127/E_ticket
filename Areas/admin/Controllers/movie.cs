using E_ticket.data;
using E_ticket.Models;
using E_ticket.Models.viewmodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_ticket.Areas.admin.Controllers
{
    [Area("admin")]
    public class movie : Controller
    {
        private ApplicationDbContext _dbContext = new();
        public IActionResult Index()
        {
            var movies = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images).Include(e => e.actorsMovies);
            return View(movies.ToList());

        }
        [HttpGet]
        public IActionResult Create()
        {
            var movie = _dbContext.Movies.ToList();
            var categore = _dbContext.Categories.Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Id.ToString()
            }).ToList();
            var cinema = _dbContext.Cinemas.Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Id.ToString()

            }).ToList();
            var actor = _dbContext.Actors.Select(e => new SelectListItem()
            {
                Text = e.FirstName + " " + e.LastName,
                Value = e.Id.ToString()
            }).ToList();
            ActorORmovies actorORmovies = new()
            {
                categories = categore.ToList(),
                actors = actor.ToList(),
                cinemas = cinema.ToList(),
                Movie = new()
            };
            return View(actorORmovies);
        }
        [HttpPost]
        public IActionResult Create(Movie movie, List<int> ActorsId, List<IFormFile> imgs)
        {
            if (ModelState.IsValid)
            {
                var categore = _dbContext.Categories.Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                }).ToList();
                var cinema = _dbContext.Cinemas.Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Id.ToString()

                }).ToList();
                var actor = _dbContext.Actors.Select(e => new SelectListItem()
                {
                    Text = e.FirstName + " " + e.LastName,
                    Value = e.Id.ToString()
                }).ToList();
                ActorORmovies actorORmovies = new()
                {
                    categories = categore.ToList(),
                    actors = actor.ToList(),
                    cinemas = cinema.ToList(),
                    Movie = new()
                };
                return View(actorORmovies);
            }
            if (imgs is not null && imgs.Count > 0)
            {
                List<string> newImgs = new List<string>();
                foreach (var item in imgs)
                {

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }
                    newImgs.Add(fileName);
                }
                _dbContext.Movies.Add(movie);
               
                _dbContext.SaveChanges();
                if (ActorsId.Any())
                {
                    foreach (var item in ActorsId)
                    {
                        _dbContext.ActorsMovies.Add(new ActorMovie() { ActorId = item, MovieId = movie.Id });
                    }
                }
                if (newImgs.Any())
                {
                    foreach (var item in newImgs)
                    {
                        _dbContext.Images.Add(new() { ImageUrl = item, MovieId = movie.Id });
                    }
                }
              
                _dbContext.SaveChanges();
                TempData["successfull notification"] = "add successfull movies";
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction(nameof(Index));


        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(e => e.Id == id);
            if (movie is not null)
            {
                var categore = _dbContext.Categories.Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                }).ToList();
                var cinema = _dbContext.Cinemas.Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Id.ToString()

                }).ToList();
                var actor = _dbContext.Actors.Select(e => new SelectListItem()
                {
                    Text = e.FirstName + " " + e.LastName,
                    Value = e.Id.ToString()
                }).ToList();
                var selectactor = _dbContext.ActorsMovies.Where(e => e.MovieId == id);
                ActorORmovies actorORmovies = new()
                {
                    categories = categore.ToList(),
                    actors = actor.ToList(),
                    cinemas = cinema.ToList(),
                    Movie = movie,
                    MyActors = selectactor.ToList()

                };
                return View(actorORmovies);
            }
            return NotFound();

        }
        [HttpPost]
        public IActionResult Edit(Movie movie, List<int> ActorsId, List<IFormFile> imgs)
        {

            var oldImgs = _dbContext.Images.Where(e => e.MovieId == movie.Id).ToList();

            if (oldImgs is not null)
            {     // validation
                if (!ModelState.IsValid)
                {
                    var categore = _dbContext.Categories.Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    }).ToList();
                    var cinema = _dbContext.Cinemas.Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()

                    }).ToList();
                    var actor = _dbContext.Actors.Select(e => new SelectListItem()
                    {
                        Text = e.FirstName + " " + e.LastName,
                        Value = e.Id.ToString()
                    }).ToList();
                    ActorORmovies actorORmovies = new()
                    {
                        categories = categore.ToList(),
                        actors = actor.ToList(),
                        cinemas = cinema.ToList(),
                        Movie = movie
                    };
                    return View(actorORmovies);
                }
                if (imgs.Any())
                {
                    List<string> newImgs = new List<string>();
                    // delete old images 


                    foreach (var item in imgs)
                    {
                        //hjksfdjghdfsiuoydfsi.png
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            item.CopyTo(stream);
                        }
                        newImgs.Add(fileName);
                    }
                    //save new images
                    foreach (var item in newImgs)
                    {
                        _dbContext.Images.Add(new() { ImageUrl = item, MovieId = movie.Id });
                    }
                    // delete imgase in database
                    foreach (var item in oldImgs)
                    {
                        // delete imgase in database
                        _dbContext.Images.Remove(item);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", item.ImageUrl);
                        //delete from wwwroot
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                //delete old actors
                if (ActorsId.Any())
                {
                    var oldActors = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id);


                    //delete ols actors
                    foreach (var item in oldActors)
                    {
                        _dbContext.ActorsMovies.Remove(item);
                    }
                    foreach (var item in ActorsId)
                    {
                        _dbContext.ActorsMovies.Add(new() { ActorId = item, MovieId = movie.Id });
                    }
                }
                _dbContext.Update(movie);
                TempData["successfull notification"] = "update succsufull movies";
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(e => e.Id == id); 
            if (movie is not null)
            {
                var oldImgs = _dbContext.Images.Where(e => e.MovieId == movie.Id).ToList();
                foreach (var item in oldImgs)
                {
                    _dbContext.Remove(item);
                    var pathimageold = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", item.ImageUrl); 
                    if (System.IO.File.Exists(pathimageold))
                    {
                        System.IO.File.Delete(pathimageold);
                    }
                }
                var oldacroid = _dbContext.ActorsMovies.Where(e => e.MovieId == movie.Id).ToList(); // Fix: Ensure IQueryable is converted to a List
                if (oldacroid is not null)
                {
                    foreach (var item in oldacroid)
                    {
                        _dbContext.ActorsMovies.Remove(item);
                    }
                }
                _dbContext.Movies.Remove(movie);
                TempData["successfull notification"] = "delete succsefull movies";
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index)); 
        }
    }
        
    
}