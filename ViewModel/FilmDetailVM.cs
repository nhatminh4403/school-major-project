using school_major_project.Models;
namespace school_major_project.ViewModel
{
    public class FilmDetailVM
    {
        public Film Film { get; set; }
        public double averageRating { get; set; } = 0;
        public IEnumerable<Rating> AllRatings { get; set; }

        public IEnumerable<Category> AllCategories { get; set; }
        public int numberOfRating { get; set; }
        public IEnumerable<string> ListOfActors { get; set; }
    }
}
