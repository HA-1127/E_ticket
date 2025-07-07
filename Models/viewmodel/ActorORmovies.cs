using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_ticket.Models.viewmodel
{
    public class ActorORmovies
    {
       public List<SelectListItem> categories { get; set; }
        public List<SelectListItem> cinemas { get; set; }
        public List<SelectListItem> actors { get; set; }
        public Movie Movie { get; set; }
        public List<ActorMovie> MyActors { get; set; }
    }
}
