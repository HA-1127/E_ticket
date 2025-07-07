namespace E_ticket.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string News { get; set; } = string.Empty;
        public List<Movie> movies { get; set; } = new List<Movie>();
        public List<ActorMovie> actorsMovies { get; set; } = new List<ActorMovie>();
    }
}
