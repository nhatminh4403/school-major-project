using school_major_project.Models;

namespace school_major_project.ViewModel
{
    public class BaseFilmVM
    {
        public IEnumerable<Film>? Films { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Country>? Countries { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
        public IEnumerable<Cinema>? Cinemas { get; set; }
        public IEnumerable<Room>? Rooms { get; set; }
        public IEnumerable<Schedule>? Schedules { get; set; }
        public IEnumerable<Seat>? Seats { get; set; }
    }
}
