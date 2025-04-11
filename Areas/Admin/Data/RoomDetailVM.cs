using school_major_project.DTO;
using school_major_project.Models;
namespace school_major_project.Areas.Admin.Data
{
    public class RoomDetailVM
    {
        public bool HasNoSchedules { get; set; } = true;
        public int? SelectedScheduleId { get; set; } = 0;
        public string RoomName { get; set; }
        public Room Room { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }

        public IEnumerable<SeatDTO> Seats { get; set; }
    }
}
