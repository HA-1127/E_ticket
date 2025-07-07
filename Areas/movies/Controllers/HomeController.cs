using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_ticket.Models;
using E_ticket.data;
using Microsoft.EntityFrameworkCore;
using E_ticket.Models.viewmodel;

namespace E_ticket.Areas.movies.Controllers;
[Area("movies")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    private readonly ApplicationDbContext _dbContext = new();
    public IActionResult Index()
   
    {
        var movies = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images);
        if (movies is not null)
        {
            return View(movies.ToList());
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    public IActionResult Details(int Id)
    {
        var movie = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images)
           .FirstOrDefault(e => e.Id == Id);
        var actormovies = _dbContext.ActorsMovies.Include(e => e.Actor).Where(e => e.MovieId == Id).Include(e => e.Actor).ToList();
        if (movie == null)
        {
            return RedirectToAction("NotFoundPage", "Home");
        }
        ModelsAndActorMovieVM modelsAndActorMovieVM = new()
        {
            movies = movie,
            actorMovies = actormovies,
        };
        return View(modelsAndActorMovieVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult NotFoundPage()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
