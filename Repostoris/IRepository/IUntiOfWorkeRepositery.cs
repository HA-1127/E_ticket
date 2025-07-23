using E_ticket.data;
using E_ticket.Models;
using Microsoft.AspNetCore.Identity;

namespace E_ticket.Repostoris.IRepository
{
    public interface IUntiOfWorkeRepositery : IDisposable
    {

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
    }
}
