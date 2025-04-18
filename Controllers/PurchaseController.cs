using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Extensions;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.PaymentMethods.VNPay;
using school_major_project.PaymentMethods.VNPay.Services;
using school_major_project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
        private readonly IVnPayService _vnPayService;

        // Session key constants
        private const string CheckoutSessionKey = "CheckoutData";

        public PurchaseController(ApplicationDbContext context, SignInManager<User> signInManager, IPromotionRepository promotionRepository,
            UserManager<User> userManager, IFoodRepository foodRepository, IReceiptRepository receiptRepository,
            IReceiptDetailsRepository receiptDetailsRepository,IVnPayService vnPayService) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
            _foodRepository = foodRepository;
            _receiptRepository = receiptRepository;
            _receiptDetailsRepository = receiptDetailsRepository;
            _vnPayService = vnPayService;
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
                    selectedSeatsList = JsonSerializer.Deserialize<List<SelectedSeatInfo>>
                        (seatSymbol, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<SelectedSeatInfo>();
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
                    userPromotions = currentUser.Promotions
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
                
            };
            ViewBag.Promotions = userPromotions;
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

            // Store checkout data in session
            var checkoutData = new CheckoutSessionData
            {
                CheckoutModel = model,
                FinalPrice = finalPrice,
                UserId = userId
            };

            HttpContext.Session.SetObjectAsJson(CheckoutSessionKey, checkoutData);

            switch (model.PaymentMethod.ToLower())
            {
                case "cash":
                    
                    var checkoutSessionData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);   
                    var ckModel = checkoutSessionData.CheckoutModel;
                    var promotion = await _promotionRepository.GetByCodeAsync(ckModel.PromotionCode);

                    if(promotion != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        var userprmo = await _context.Users.Include(u => u.Promotions).Where(p=> p.Promotions.Any(p => p.Id == promotion.Id)).FirstOrDefaultAsync();
                        if(userprmo != null)
                        {
                            user.Promotions.Remove(promotion);
                            await _userManager.UpdateAsync(user);
                        }
                    }

                    await SaveReceiptAsync(model, finalPrice, userId);
                    TempData["PaymentSuccessMessage"] = "Đặt vé thành công! Vui lòng thanh toán tại quầy.";
                    return RedirectToAction("PaymentSuccess", "Purchase");

                case "momo":
                    return RedirectToAction("ProcessMomoPayment");

                case "paypal":
                    return RedirectToAction("ProcessPayPalPayment");

                case "vnpay":
                    return RedirectToAction("ProcessVNPayPayment");

                default:
                    TempData["Message"] = "Phương thức thanh toán không hợp lệ.";
                    return View("Add", model);
            }
        }
        [Route("thanh-toan-vnpay")]
        public async Task<IActionResult> ProcessVNPayPayment()
        {
            // Get checkout data from session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.GetUserAsync(User);

            // Tạo thông tin cho thanh toán VNPay, nhưng CHƯA lưu vào database
            var vnPayModel = new VnPaymentRequestModel
            {
                Price = (double)checkoutData.FinalPrice,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán vé xem phim cho {user.FullName}",
                FullName = user.FullName,
                OrderId = new Random().Next(1000, 10000)
            };

            // Lưu thông tin vào session để sử dụng sau khi VNPay callback
            HttpContext.Session.SetObjectAsJson("PendingCheckoutData", checkoutData);

            // Tạo URL thanh toán VNPay
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);

            return Redirect(paymentUrl);
        }


        // Hàm chung tính giá sau giảm giá & combo
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

        // Hàm chung lưu Receipt + ReceiptDetails - now returns the created receipt
        private async Task<Receipt> SaveReceiptAsync(CheckoutSummaryVM model, decimal finalPrice, string userId)
        {
            // Tạo Receipt
            var receipt = new Receipt
            {
                Date = DateTime.Now,
                TotalPrice = (int)finalPrice,
                PaymentType = model.PaymentMethod,
                ComboFoodId = ParseComboId(model.ComboIdAndPrice),
                UserId = userId,
                IsPaid = model.PaymentMethod != "Cash" // Chỉ đánh dấu đã thanh toán nếu không phải thanh toán tiền mặt
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
                await _receiptDetailsRepository.AddAsync(detail);
            }

            await _context.SaveChangesAsync();
            return receipt;
        }

        private int? ParseComboId(string combo)
        {
            if (string.IsNullOrEmpty(combo) || combo == "0-0") return null;
            var parts = combo.Split('-');
            if (int.TryParse(parts[0], out var id)) return id;
            return null;
        }

        [Authorize]
        [HttpGet]
        [Route("vnpay-payment-callback")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "không xác định"}";
                return RedirectToAction("PaymentFail");
            }

            // Lấy lại thông tin từ Session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>("PendingCheckoutData");

            if (checkoutData == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin giao dịch để xác thực.";
                return RedirectToAction("PaymentFail");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Xử lý mã giảm giá nếu có
                    var ckModel = checkoutData.CheckoutModel;
                    var promotion = await _promotionRepository.GetByCodeAsync(ckModel.PromotionCode);

                    if (promotion != null)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        var userprmo = await _context.Users.Include(u => u.Promotions)
                            .Where(p => p.Promotions.Any(p => p.Id == promotion.Id))
                            .FirstOrDefaultAsync();

                        if (userprmo != null)
                        {
                            user.Promotions.Remove(promotion);
                            await _userManager.UpdateAsync(user);
                        }
                    }

                    // Tạo Receipt
                    var receipt = new Receipt
                    {
                        Date = DateTime.Now,
                        TotalPrice = (int)checkoutData.FinalPrice,
                        PaymentType = "VNPay",
                        ComboFoodId = ParseComboId(checkoutData.CheckoutModel.ComboIdAndPrice),
                        UserId = checkoutData.UserId,
                        IsPaid = true // Đánh dấu đã thanh toán vì đã nhận được xác nhận từ VNPay
                    };
                    await _receiptRepository.AddAsync(receipt);

                    // Tạo ReceiptDetails
                    foreach (var seat in checkoutData.CheckoutModel.SelectedSeats)
                    {
                        var detail = new ReceiptDetail
                        {
                            ReceiptId = receipt.Id,
                            FilmName = checkoutData.CheckoutModel.FilmTitle,
                            CinemaName = checkoutData.CheckoutModel.CinemaName,
                            RoomName = checkoutData.CheckoutModel.RoomName,
                            CinemaAddress = checkoutData.CheckoutModel.CinemaAddress,
                            StartTime = checkoutData.CheckoutModel.StartTime,
                            PricePerSeat = (int)seat.Price,
                            SeatId = Convert.ToInt32(seat.Id),
                            ScheduleId = checkoutData.CheckoutModel.ScheduleId,
                            PosterUrl = checkoutData.CheckoutModel.PosterUrl,
                            SeatName = seat.Symbol
                        };
                        await _receiptDetailsRepository.AddAsync(detail);
                    }

                    // Lưu xuống database
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Xóa session sau khi xử lý xong
                    HttpContext.Session.Remove("PendingCheckoutData");

                    TempData["SuccessMessage"] = "Thanh toán VNPay thành công!";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    // TODO: Log chi tiết lỗi ở đây
                    TempData["ErrorMessage"] = "Có lỗi trong quá trình xử lý giao dịch.";
                    return RedirectToAction("PaymentFail");
                }
            }
        }

        [Route("thanh-toan-momo")]
        public IActionResult ProcessMomoPayment()
        {
            // Get checkout data from session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }

            // Implement MoMo payment logic here
            // ...

            TempData["SuccessMessage"] = "Thanh toán MoMo thành công!";
            HttpContext.Session.Remove(CheckoutSessionKey);
            return RedirectToAction("History", "User");
        }
        [Route("thanh-toan-paypal")]
        public IActionResult ProcessPayPalPayment()
        {
            // Get checkout data from session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }



            TempData["SuccessMessage"] = "Thanh toán PayPal thành công!";
            HttpContext.Session.Remove(CheckoutSessionKey);
            return RedirectToAction("History", "User");
        }
        [Route("thanh-toan-that-bai")]
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Route("thanh-toan-thanh-cong")]
        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }

    // Class to store checkout data in session
    public class CheckoutSessionData
    {
        public CheckoutSummaryVM CheckoutModel { get; set; }
        public decimal FinalPrice { get; set; }
        public string UserId { get; set; }
    }
}