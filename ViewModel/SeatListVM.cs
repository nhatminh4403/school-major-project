using school_major_project.Models;

namespace school_major_project.ViewModel
{
    public class SeatListVM
    {
        public Schedule Schedule { get; set; }
        public List<Seat> SeatsWithStatus { get; set; }
        public Dictionary<string, List<Seat>> SeatsByType { get; set; }
        //public List<Category> Categories { get; set; }
        public string CurrentTime { get; set; }

        public Film Film;
        public Room Room;
        public Cinema Cinema;
        public long SelectedScheduleId;
        public long SelectedRoomId;
        public string CinemaName;
        public string CinemaAddress;
        public string RoomName;


    }
}
