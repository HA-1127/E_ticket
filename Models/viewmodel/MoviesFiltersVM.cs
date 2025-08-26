namespace E_ticket.Models.viewmodel
{
    public class MoviesFiltersVM
    {
        public string? NameMovies { get; set; }
        public int? CatogeryId { get; set; }
        public int? CinemaId { get; set; }
        public bool? Stutas { get; set; }
        public double? MaxPrice { get; set; }
        public double? MinPrice { get; set; }
     
        public int? ActorId { get; set; }
    }
}
