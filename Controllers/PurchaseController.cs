using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Text.Json;

namespace school_major_project.Controllers
{
    [Route("/thanh-toan")]
    [Authorize]
    public class PurchaseController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailsRepository _receiptDetailsRepository;
       
        public PurchaseController(ApplicationDbContext context, SignInManager<User> signInManager, IPromotionRepository promotionRepository,
            UserManager<User> userManager, IFoodRepository foodRepository, IReceiptRepository receiptRepository,
            IReceiptDetailsRepository receiptDetailsRepository) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
            _foodRepository = foodRepository;
            _receiptRepository = receiptRepository;
            _receiptDetailsRepository = receiptDetailsRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("")]
        public async Task<IActionResult> Add(string seatSymbol, decimal totalPrice, DateTime startTime, string filmTitle,
                                                string poster, List<string> category, string cinemaName,
                                                string cinemaAddress, string roomName, int scheduleId)
        {
            // --- Deserialize Seats (same as before) ---
            List<SelectedSeatInfo> selectedSeatsList = new List<SelectedSeatInfo>();
            if (!string.IsNullOrEmpty(seatSymbol))
            {
                try
                {
                    selectedSeatsList = JsonSerializer.Deserialize<List<SelectedSeatInfo>>(seatSymbol, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<SelectedSeatInfo>();
                }
                catch (JsonException ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý thông tin ghế đã chọn.";
                    return NotFound("Invalid seat data format.");
                }
            }
            if (selectedSeatsList.Count == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một ghế.";
                return NotFound("No seats selected.");
            }

            List<Promotion> userPromotions = new List<Promotion>();
            var userId = _userManager.GetUserId(User);

            if (!string.IsNullOrEmpty(userId))
            {
                // Eagerly load the Promotions navigation property
                var currentUser = await _context.Users
                                          .Include(u => u.Promotions)
                                          .FirstOrDefaultAsync(u => u.Id == userId);

                if (currentUser != null && currentUser.Promotions != null)
                {
                    userPromotions = currentUser.Promotions// .Where(p => p.IsActive && p.ExpiryDate > DateTime.UtcNow) // Optional filtering
                                                .ToList();
                }
            }
            else
            {
                Console.WriteLine("User not logged in, cannot fetch user-specific promotions.");
            }

            var comboFoods = await _foodRepository.GetAllAsync();
            ViewBag.ComboFoods = comboFoods;
            // --- Create ViewModel ---
            var checkoutViewModel = new CheckoutSummaryVM
            {
                SelectedSeats = selectedSeatsList,
                TotalPrice = totalPrice,
                StartTime = startTime,
                FilmTitle = filmTitle,
                PosterUrl = poster,
                Categories = category ?? new List<string>(),
                CinemaName = cinemaName,
                CinemaAddress = cinemaAddress,
                RoomName = roomName,
                ScheduleId = scheduleId,
                Promotions = userPromotions,
                
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        [Route("thanh-toan")]
        public async Task<IActionResult> Checkout(CheckoutSummaryVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PurchaseMessage"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
                return View("Add", model);
            }

            // 1. Lấy user hiện tại
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                return RedirectToAction("Login", "Account");
            }

            // 2. Tính giá cuối cùng (discount + combo)
            decimal finalPrice = CalculateFinalPrice(model);

            switch (model.PaymentMethod)
            {
                case "Cash":
                    // Thanh toán tại quầy: tạo và lưu luôn
                    await SaveReceiptAsync(model, finalPrice, userId);
                    TempData["SuccessMessage"] = "Đặt vé thành công! Vui lòng thanh toán tại quầy.";
                    return RedirectToAction("History", "User");

                case "momo":
                    // Chuẩn bị dữ liệu cho callback MOMO
                    TempData["CheckoutData"] = JsonSerializer.Serialize(new
                    {
                        Model = model,
                        FinalPrice = finalPrice,
                        UserId = userId
                    });
                    return RedirectToAction("ProcessMomoPayment");

                case "paypal":
                    TempData["CheckoutData"] = JsonSerializer.Serialize(new
                    {
                        Model = model,
                        FinalPrice = finalPrice,
                        UserId = userId
                    });
                    return RedirectToAction("ProcessPayPalPayment");

                case "vnpay":
                    TempData["CheckoutData"] = JsonSerializer.Serialize(new
                    {
                        Model = model,
                        FinalPrice = finalPrice,
                        UserId = userId
                    });
                    return RedirectToAction("ProcessVNPayPayment");

                default:
                    TempData["Message"] = "Phương thức thanh toán không hợp lệ.";
                    return View("Add", model);
            }
        }
        // Additional methods for handling payment gateways
        [HttpPost]
        [Route("thanh-toan-momo")]
        public async Task<IActionResult> ProcessMomoPayment()
        {
            // TODO: gọi API MOMO, kiểm tra kết quả  
            bool paymentSuccess = /* gọi MOMO, nhận kết quả */true;

            if (paymentSuccess && TempData["CheckoutData"] != null)
            {
                // Deserialize dữ liệu  
                var payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                                    TempData["CheckoutData"]!.ToString()!);
                var model = JsonSerializer.Deserialize<CheckoutSummaryVM>(
                                payload["Model"].GetRawText())!;
                var finalPrice = payload["FinalPrice"].GetDecimal();
                var userId = payload["UserId"].GetString()!;

                // Lưu Receipt và ReceiptDetails  
                await SaveReceiptAsync(model, finalPrice, userId);

                TempData["SuccessMessage"] = "Thanh toán MOMO thành công!";
                return RedirectToAction("History", "User");
            }
            else
            {
                TempData["Message"] = "Thanh toán MOMO thất bại, vui lòng thử lại.";
                return RedirectToAction("Add"); // Removed 'model' parameter to fix CS1739  
            }
        }

        [HttpPost]
        [Route("thanh-toan-paypal")]
        public IActionResult ProcessPayPalPayment(int receiptId)
        {
            // Similar implementation for PayPal
            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán PayPal thành công!";
            return RedirectToAction("History", "User");
        }

        [HttpPost]
        [Route("thanh-toan-vnpay")]
        public IActionResult ProcessVNPayPayment(int receiptId)
        {
            // Similar implementation for VNPay
            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán VNPay thành công!";
            return RedirectToAction("History", "User");
        }
        private decimal CalculateFinalPrice(CheckoutSummaryVM model)
        {
            decimal price = model.TotalPrice;
            if (!string.IsNullOrEmpty(model.AppliedPromoCode) && model.AppliedDiscountRate > 0)
            {
                price -= price * (decimal)model.AppliedDiscountRate;
            }
            if (!string.IsNullOrEmpty(model.ComboIdAndPrice) && model.ComboIdAndPrice != "0-0")
            {
                var parts = model.ComboIdAndPrice.Split('-');
                if (decimal.TryParse(parts[1], out var comboPrice))
                    price += comboPrice;
            }
            return price;
        }

        // Hàm chung lưu Receipt + ReceiptDetails
        private async Task SaveReceiptAsync(CheckoutSummaryVM model, decimal finalPrice, string userId)
        {
            // Tạo Receipt
            var receipt = new Receipt
            {
                Date = DateTime.Now,
                TotalPrice = (int)finalPrice,
                PaymentType = model.PaymentMethod,
                ComboFoodId = ParseComboId(model.ComboIdAndPrice),
                UserId = userId,
                IsPaid = true  // đã qua thanh toán
            };
            await _receiptRepository.AddAsync(receipt);

            // Tạo ReceiptDetails
            foreach (var seat in model.SelectedSeats)
            {
                var detail = new ReceiptDetail
                {
                    ReceiptId = receipt.Id,
                    FilmName = model.FilmTitle,
                    CinemaName = model.CinemaName,
                    RoomName = model.RoomName,
                    CinemaAddress = model.CinemaAddress,
                    StartTime = model.StartTime,
                    PricePerSeat = (int)seat.Price,
                    SeatId = Convert.ToInt32(seat.Id),
                    ScheduleId = model.ScheduleId,
                    PosterUrl = model.PosterUrl,
                    SeatName = seat.Symbol
                };
                _context.ReceiptDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
        }

        private int? ParseComboId(string combo)
        {
            if (string.IsNullOrEmpty(combo) || combo == "0-0") return null;
            var parts = combo.Split('-');
            if (int.TryParse(parts[0], out var id)) return id;
            return null;
        }
    }

}
