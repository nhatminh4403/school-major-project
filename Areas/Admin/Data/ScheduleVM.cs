using school_major_project.Models;

namespace school_major_project.Areas.Admin.Data
{
    public class ScheduleVM
    {
        public IEnumerable<Schedule>? Schedules { get; set; }
        public IEnumerable<Film>? ReleasedFilmsWithoutSchedules { get; set; }
        public IEnumerable<Film>? UpcomingFilms { get; set; }
    }
}
