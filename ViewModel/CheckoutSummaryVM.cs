using school_major_project.Models;
using System;
using System.Collections.Generic;

namespace school_major_project.ViewModel
{
    public class CheckoutSummaryVM
    {
        public List<SelectedSeatInfo> SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartTime { get; set; }
        public string FilmTitle { get; set; }
        public string PosterUrl { get; set; }
        public List<string> Categories { get; set; }
        public string CinemaName { get; set; }
        public string CinemaAddress { get; set; }
        public string RoomName { get; set; }
        public int ScheduleId { get; set; }


        public string? AppliedPromoCode { get; set; }
        public double AppliedDiscountRate { get; set; } 
        public string PromotionCode { get; set; }
        public string PaymentMethod { get; set; }
        public string ComboIdAndPrice { get; set; }

        public CheckoutSummaryVM()
        {
            SelectedSeats = new List<SelectedSeatInfo>();
            Categories = new List<string>();
        }
    }
}
