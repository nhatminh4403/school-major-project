namespace BookingMovieTickets.Extensions
{
    public class FilmNotFoundException : Exception
    {
        public FilmNotFoundException(string message) : base(message)
        {
        }
    }
}
