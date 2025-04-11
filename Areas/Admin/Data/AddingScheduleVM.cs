using Microsoft.AspNetCore.Mvc.Rendering;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Data
{
    public class AddingScheduleVM
    {
        public Schedule Schedule { get; set; }

        public IEnumerable<SelectListItem> FilmList { get; set; }

        public IEnumerable<SelectListItem> RoomList { get; set; }


        public AddingScheduleVM()
        {
            Schedule = new Schedule();
            FilmList = new List<SelectListItem>();
            RoomList = new List<SelectListItem>();
        }
    }
}
