using school_major_project.Models;

namespace school_major_project.Areas.Admin.Data
{
    public class CinemasViewModel
    {
        public IEnumerable<Cinema> Cinemas { get; set; }
        public Cinema SelectedCinema { get; set; }
    }
}
