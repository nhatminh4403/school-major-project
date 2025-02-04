using school_major_project.Models;

namespace school_major_project.ViewModel
{
    public class FilmPagingViewModel
    {
        public IEnumerable<Film> Films { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
