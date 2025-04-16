using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Net.Sockets;
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
                    selectedSeatsList = JsonSerializer.Deserialize<List<SelectedSeatInfo>>(seatSymbol, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<SelectedSeatInfo>();
                }
                catch (JsonException ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý thông tin ghế đã chọn.";
                    return BadRequest("Invalid seat data format.");
                }
            }
            if (selectedSeatsList.Count == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một ghế.";
                return BadRequest("No seats selected.");
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
                Foods = comboFoods
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutSummaryVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PurchaseMessage"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
                return View(model);
            }
            try
            {
                // Get current user
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    TempData["Message"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                    return RedirectToAction("Login", "Account");
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Message"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Login", "Account");
                }

                // Calculate final price with discount if applicable
                decimal finalPrice = model.TotalPrice;

                // Apply promo code discount if exists
                if (!string.IsNullOrEmpty(model.AppliedPromoCode) && model.AppliedDiscountRate > 0)
                {
                    // Get the promotion from repository to verify it
                    var promotion = await _promotionRepository.GetByCodeAsync(model.AppliedPromoCode);
                    if (promotion != null && promotion.StartDate <= DateTime.Now && promotion.EndDate >= DateTime.Now)
                    {
                        finalPrice -= finalPrice * (decimal)model.AppliedDiscountRate;

                        // Remove the used promotion from user
                        if (user.Promotions != null)
                        {
                            var userPromotion = user.Promotions.FirstOrDefault(p => p.Code == model.AppliedPromoCode);
                            if (userPromotion != null)
                            {
                                user.Promotions.Remove(userPromotion);
                                await _userManager.UpdateAsync(user);
                            }
                        }
                    }
                }

                // Handle combo food selection
                int comboId = 0;
                decimal comboPrice = 0;
                if (!string.IsNullOrEmpty(model.ComboIdAndPrice) && model.ComboIdAndPrice != "0-0")
                {
                    var comboParts = model.ComboIdAndPrice.Split('-');
                    if (comboParts.Length == 2 && int.TryParse(comboParts[0], out comboId) && decimal.TryParse(comboParts[1], out comboPrice))
                    {
                        finalPrice += comboPrice;
                    }
                }

                // Create receipt
                var receipt = new Receipt
                {
                    Date = DateTime.Now,
                    TotalPrice = (int)finalPrice, // Convert decimal to int as per model
                    PaymentType = model.PaymentMethod, // Use PaymentType as per model
                    SeatName = string.Join(", ", model.SelectedSeats.Select(s => s.Symbol)),
                    ComboFoodId = comboId > 0 ? comboId : 0, // Set to 0 if no combo selected
                    UserId = userId
                };

                await _receiptRepository.AddAsync(receipt);

                // Create tickets for each seat
                if (model.SelectedSeats != null && model.SelectedSeats.Any())
                {
                    foreach (var seat in model.SelectedSeats)
                    {
                        var receiptDetail = new ReceiptDetail
                        {
                            ReceiptId = receipt.Id,
                            FilmName = model.FilmTitle,
                            CinemaName = model.CinemaName,
                            RoomName = model.RoomName,
                            CinemaAddress = model.CinemaAddress,
                            StartTime = model.StartTime,
                            PricePerSeat = (int)seat.Price,
                            SeatId = seat.Id,
                            ScheduleId = model.ScheduleId
                        };
                        _context.ReceiptDetails.Add(receiptDetail);
                    }
                }
                await _context.SaveChangesAsync();

                // Handle different payment methods
                switch (model.PaymentMethod)
                {
                    case "momo":
                        // Redirect to MOMO payment gateway
                        return RedirectToAction("ProcessMomoPayment", new { receiptId = receipt.Id });
                    case "paypal":
                        // Redirect to PayPal payment gateway
                        return RedirectToAction("ProcessPayPalPayment", new { receiptId = receipt.Id });
                    case "vnpay":
                        // Redirect to VNPay payment gateway
                        return RedirectToAction("ProcessVNPayPayment", new { receiptId = receipt.Id });
                    case "Cash":
                    default:
                        // Cash payment - show success message and redirect to receipt page
                        TempData["SuccessMessage"] = "Đặt vé thành công! Vui lòng thanh toán tại quầy.";
                        return RedirectToAction("History", "User");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Có lỗi xảy ra: {ex.Message}";
                return View(model);
            }
        }
        // Additional methods for handling payment gateways
        public IActionResult ProcessMomoPayment(int receiptId)
        {
            // Implement MOMO payment integration
            // This is just a placeholder - you'll need to implement actual MOMO API integration

            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán MOMO thành công!";
            return RedirectToAction("History", "User");
        }

        public IActionResult ProcessPayPalPayment(int receiptId)
        {
            // Similar implementation for PayPal
            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán PayPal thành công!";
            return RedirectToAction("History", "User");
        }

        public IActionResult ProcessVNPayPayment(int receiptId)
        {
            // Similar implementation for VNPay
            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán VNPay thành công!";
            return RedirectToAction("History", "User");
        }
    }

}
