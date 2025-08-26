using E_ticket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_ticket.Models.viewmodel;

namespace E_ticket.data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Option) : base(Option)
        {

        }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovie> ActorsMovies { get; set; }
        public DbSet<MovieImage> Images { get; set; }
        public DbSet<UserOtp> userOtps { get; set; }
        public DbSet<MoviesUserTicket> moviesUserTickets { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<TicketItme>ticketItmes { get; set; }
    
      
      
        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=E-ticket514;Integrated Security=True;Trust Server Certificate=True");
        }
        
        
    }
}
