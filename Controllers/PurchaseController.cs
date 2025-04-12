using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace school_major_project.Controllers
{
    [Route("/thanh-toan")]
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Add(
            string seatSymbol,
            decimal totalPrice,
            DateTime startTime,
            string filmTitle,
            string poster,
            string[] category,
            string cinemaName,
            string cinemaAddress,
            string roomName,
            int scheduleId)
        {
            // Tạo view model để hiển thị thông tin đặt vé
            var purchaseVM = new PurchaseViewModel
            {
                SelectedSeats = seatSymbol?.Split(',').ToList() ?? new List<string>(),
                TotalPrice = totalPrice,
                StartTime = startTime,
                FilmTitle = filmTitle,
                PosterUrl = poster,
                Categories = category?.ToList() ?? new List<string>(),
                CinemaName = cinemaName,
                CinemaAddress = cinemaAddress,
                RoomName = roomName,
                ScheduleId = scheduleId
            };

            // Lưu thông tin vào session để sử dụng ở bước tiếp theo
            HttpContext.Session.SetString("PurchaseInfo", System.Text.Json.JsonSerializer.Serialize(purchaseVM));

            return View(purchaseVM);
        }
    }
}
