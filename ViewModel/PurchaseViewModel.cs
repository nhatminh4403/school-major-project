using System;
using System.Collections.Generic;

namespace school_major_project.ViewModel
{
    public class PurchaseViewModel
    {
        public List<string> SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartTime { get; set; }
        public string FilmTitle { get; set; }
        public string PosterUrl { get; set; }
        public List<string> Categories { get; set; }
        public string CinemaName { get; set; }
        public string CinemaAddress { get; set; }
        public string RoomName { get; set; }
        public int ScheduleId { get; set; }
    }
    public class SeatSelectionInfo
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public string SeatImage { get; set; }
        public int SeatTypeId { get; set; }
    }
}
