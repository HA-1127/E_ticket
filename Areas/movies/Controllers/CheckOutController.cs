using E_ticket.Models;
using E_ticket.Repostoris.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Stripe.BillingPortal;
using Stripe.Climate;
using System.Threading.Tasks;

namespace E_ticket.Areas.movies.Controllers
{
    [Area("movies")]
    public class CheckOutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITicketItemRepository _ticketItemRepository;
        private readonly IMoviesUserTicketRepositiriy _moviesUserTicketRepositiriy;
        private readonly ITicketRepository _ticketRepository;

        public CheckOutController(UserManager<ApplicationUser> userManager, ITicketRepository ticketRepository,
            ITicketItemRepository ticketItemRepository, IMoviesUserTicketRepositiriy moviesUserTicketRepositiriy)
        {
            _userManager = userManager;
            _ticketRepository = ticketRepository;
            _ticketItemRepository = ticketItemRepository;
            _moviesUserTicketRepositiriy = moviesUserTicketRepositiriy;
        }

        public async Task<IActionResult> Success(int TicketId)
        {
         var ticket =await _ticketRepository.GetOneAsync(e => e.Id == TicketId);

            if (ticket is null)
            {
                return NotFound();
            }
            //update ticket
            ticket.ticketStatus = TicketStatus.processing;

            var service = new SessionService();
          //  var session = service.Get(ticket.SessionId);
           // ticket.TransactionId = session.PaymentIntentId;
            // movise => ticket
            var user =await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                return NotFound();
            }
            var moviestiket =await _moviesUserTicketRepositiriy.GetAsync(e => e.ApplicationUserId == user.Id
            , includes:[ e=>e.movie]);
            var ticketitem = moviestiket.Select(e => new TicketItme()
            {

                TicketId = TicketId,
                MovieId = e.MovieId,
                Quantity = e.Count,
                Price = e.movie.Price

            }).ToList();
         await _ticketItemRepository.CreatRangeAsyac(ticketitem);
            foreach (var item in moviestiket)
            {

                _moviesUserTicketRepositiriy.Delete(item);
            }
          await  _moviesUserTicketRepositiriy.CommitAsync();
           await _ticketItemRepository.CommitAsync();
           await _ticketRepository.CommitAsync();
            

            return View();
        }

        public async Task<IActionResult> Cancel(int TicketId) 
        {
           
            var ticket = await _ticketRepository.GetOneAsync(e => e.Id == TicketId);

            if (ticket is null)
            {
                return NotFound();
            }
            
                ticket.ticketStatus = TicketStatus.canceled;
           await _ticketRepository.CommitAsync();
            return View();

            
        }
    }
}
