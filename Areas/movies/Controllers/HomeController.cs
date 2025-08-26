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
    public IActionResult Index(MoviesFilterVM moviesFilterVM , int page = 1)
   
    {
        var movies = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category).Include(e => e.Images).ToList();

       //filters
        if (moviesFilterVM.NmaneMovie is not null)
        {
            movies = movies.Where(e => e.Name.Contains(moviesFilterVM.NmaneMovie)).ToList();
            ViewBag.NameMovies = moviesFilterVM.NmaneMovie;
        }
        if (moviesFilterVM.MaxPrice is not null)
        {
            movies = movies.Where(e => e.Price <= moviesFilterVM.MaxPrice).ToList();
            ViewBag.MaxPrice = moviesFilterVM.MaxPrice;
        }
        if (moviesFilterVM.Minprice is not null)
        {
            movies = movies.Where(e => e.Price >= moviesFilterVM.Minprice).ToList();
            ViewBag.Minprice = moviesFilterVM.Minprice;
        }
        var cinema = _dbContext.Cinemas.ToList();
        if (moviesFilterVM.CinemaId !=null)
        {
            if (moviesFilterVM.CinemaId > 0 && cinema.Count() >= moviesFilterVM.CinemaId)
            {
                movies = movies.Where(e => e.CinemaId == moviesFilterVM.CinemaId).ToList();
                ViewBag.CineamId = moviesFilterVM.CinemaId;
            }
        }
        //paginatio
        if (page < 0)
        {
            page = 1;
        }
        var TotallNumberOfPage = Math.Ceiling(movies.Count() / 9.0);
        movies = movies.Skip((page - 1) * 9).Take(9).ToList();
        ViewBag.toallnumberofpage = TotallNumberOfPage;
        ViewBag.currentPage = page;

          ViewBag.cinema = cinema;
            return View(movies);
        
       // return RedirectToAction("NotFoundPage", "Home");
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
