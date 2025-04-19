namespace school_major_project.Extensions
{
    public class FilmNotFoundException : Exception
    {
        public FilmNotFoundException(string message) : base(message)
        {
        }
    }
}
