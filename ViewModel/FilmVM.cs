using school_major_project.Models;

namespace school_major_project.ViewModel
{
    public class FilmVM
    {
        public IEnumerable<Film> Films { get; set; }    
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
