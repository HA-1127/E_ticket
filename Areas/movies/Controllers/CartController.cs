using E_ticket.Models;
using E_ticket.Repostoris;
using E_ticket.Repostoris.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace E_ticket.Areas.movies.Controllers
{
    [Area("movies")]
    public class CartController : Controller
    {
        private readonly IMoviesUserTicketRepositiriy _moviesUserTicketRepositiriy;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Imovierepository _movierepository;
        private readonly ITicketRepository _ticketRepository;

        public CartController(IMoviesUserTicketRepositiriy moviesUserTicketRepositiriy,
            UserManager<ApplicationUser> userManager , Imovierepository movierepository,
            ITicketRepository ticketRepository )
        {
            _moviesUserTicketRepositiriy = moviesUserTicketRepositiriy;
            _userManager = userManager;
            _movierepository = movierepository;
            _ticketRepository = ticketRepository;
        }
        public async Task<IActionResult> AddToCart(int moviesid ,int count)
        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var ticketdb = await _moviesUserTicketRepositiriy.GetOneAsync(e => e.MovieId == moviesid && 
            e.ApplicationUserId == user.Id.ToString());
            if (ticketdb is not null)
            {
                ticketdb.Count += count;
            }
            else
            {
               await _moviesUserTicketRepositiriy.CreateAsync(new ()
                {
                    ApplicationUserId = user.Id.ToString(),
                    MovieId = moviesid,
                    Count = count

                });
            }
            await _moviesUserTicketRepositiriy.CommitAsync();
            TempData["successfull notification"] = "succseefully add to ticket";
            return RedirectToAction("Index", "Home", new { area = "movies" });
        }

        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var Movisetiket = await _moviesUserTicketRepositiriy.GetAsync(e => e.ApplicationUserId == user.Id.ToString(), includes: [e=>e.movie]);
            ViewBag.totalPrice = Movisetiket.Sum(e => e.movie.Price * e.Count);

            return View(Movisetiket);
        }
        public async Task<IActionResult> InceremantCount( int moviesid)

        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var ticketdb = await _moviesUserTicketRepositiriy.GetOneAsync(e => e.MovieId == moviesid &&
           e.ApplicationUserId == user.Id.ToString());
            if (ticketdb is null)
                return NotFound();

            ticketdb.Count++;
            
          await  _moviesUserTicketRepositiriy.CommitAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> DecremantCount(int moviesid)
        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var ticketdb = await _moviesUserTicketRepositiriy.GetOneAsync(e => e.MovieId == moviesid &&
          e.ApplicationUserId == user.Id.ToString());
            if (ticketdb is null)
                return NotFound();
            if (ticketdb.Count > 1)
            {
                ticketdb.Count--;
                await _moviesUserTicketRepositiriy.CommitAsync();
            }
         
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> DeleteMovies(int moviesid)
        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var ticketdb = await _moviesUserTicketRepositiriy.GetOneAsync(e => e.MovieId == moviesid &&
        e.ApplicationUserId == user.Id.ToString());
            if (ticketdb is null)
                return NotFound();
            _moviesUserTicketRepositiriy.Delete(ticketdb);
         await   _moviesUserTicketRepositiriy.CommitAsync();
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Pay()
        {
            var user = _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            var moviesticket =await _moviesUserTicketRepositiriy.GetAsync(e => e.ApplicationUserId == user.Id.ToString(), includes: [e=>e.movie]);
            
            await _ticketRepository.CreateAsync(new()
            { IdApplicationUser = user.Id,
                ticketStatus = TicketStatus.pending,
                DateTime = DateTime.UtcNow,
                paymenMethod = PaymenMethod.Vias,

            });
           await _ticketRepository.CommitAsync();
            var ticket = (await _ticketRepository.GetAsync(e => e.IdApplicationUser == user.Id)).OrderBy(e => e.Id).LastOrDefault();
    
            if (ticket is null)
            {
                return NotFound();
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/CheckOut/Success?TicketId={ticket.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/CheckOut/Cancel?TicketId={ticket.Id}",
            };

            foreach (var item in moviesticket)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.movie.Name,
                            Description = item.movie.Description
                        },
                        UnitAmount = (long)item.movie.Price * 100, // 400.00
                    },
                    Quantity = item.Count,
                });
            }


            var service = new SessionService();
            var session = service.Create(options);
           // ticket.SessionId = session.Id;
           await _ticketRepository.CommitAsync();
            
            return Redirect(session.Url);
        }

    }
}
