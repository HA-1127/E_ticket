using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;
using Microsoft.AspNetCore.Identity;

namespace E_ticket.Repostoris
{
    public class UnitOfWorkRepository : IUntiOfWorkeRepositery
    {
        public UnitOfWorkRepository(Iactoryrepository actoryrepository,
            ICategotyRepository categotyRepository,
            IcinemaRepository cinemaRepository,
            Imovierepository movierepository,
            IMoviesUserTicketRepositiriy moviesUserTicketRepositiriy,
            IRuserotprepostoity ruserotprepostoity,
            ITicketItemRepository ticketItemRepository,
            ITicketRepository ticketRepository,
            UserManager<ApplicationUser>userManager,
            ApplicationDbContext context)
        {
            Actoryrepository = actoryrepository;
            CategotyRepository = categotyRepository;
            CinemaRepository = cinemaRepository;
            Movierepository = movierepository;
            MoviesUserTicketRepositiriy = moviesUserTicketRepositiriy;
            Ruserotprepostoity = ruserotprepostoity;
            TicketItemRepository = ticketItemRepository;
            TicketRepository = ticketRepository;
            UserManager = userManager;
            Context = context;
        }

        public Iactoryrepository Actoryrepository { get; }
        public ICategotyRepository CategotyRepository { get; }
        public IcinemaRepository CinemaRepository { get; }
        public Imovierepository Movierepository { get; }
        public IMoviesUserTicketRepositiriy MoviesUserTicketRepositiriy { get; }
        public IRuserotprepostoity Ruserotprepostoity { get; }
        public ITicketItemRepository TicketItemRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public ApplicationDbContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
