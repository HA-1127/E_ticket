using E_ticket.Areas.admin.Controllers;
using E_ticket.Models;

namespace E_ticket.Models.viewmodel
{
    public class ModelsAndActorMovieVM
    {
        public Movie movies { get; set; }
     
        public List<ActorMovie> actorMovies { get; set; }
    }
}
