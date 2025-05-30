﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Extensions;
using school_major_project.GlobalServices;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.PaymentMethods.MoMo.Models;
using school_major_project.PaymentMethods.MoMo.Services;
using school_major_project.PaymentMethods.PayPal;
using school_major_project.PaymentMethods.VNPay;
using school_major_project.PaymentMethods.VNPay.Services;
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
        private readonly IVnPayService _vnPayService;
        private readonly IPayPalService _payPalService;
        // Session key constants
        private const string CheckoutSessionKey = "CheckoutData";
        private readonly IMoMoService _momoService;
        public PurchaseController(ApplicationDbContext context, SignInManager<User> signInManager, IPromotionRepository promotionRepository,
            UserManager<User> userManager, IFoodRepository foodRepository, IReceiptRepository receiptRepository,
            IReceiptDetailsRepository receiptDetailsRepository,
            IVnPayService vnPayService, IPayPalService payPalService, IMoMoService momoService) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
            _foodRepository = foodRepository;
            _receiptRepository = receiptRepository;
            _receiptDetailsRepository = receiptDetailsRepository;
            _vnPayService = vnPayService;
            _payPalService = payPalService;
            _momoService = momoService;
        }
        #region Summary
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
                    var user = await _userManager.GetUserAsync(User);
                    var seatIds = ckModel.SelectedSeats.Select(ss => Convert.ToInt32(ss.Id)).ToList();

                    var seats = await _context.Seats
                        .Include(s => s.SeatType)
                        .Where(s => seatIds.Contains(s.SeatId))
                        .ToListAsync();

                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }
                    if (promotion != null)
                    {
                        var userprmo = await _context.Users.Include(u => u.Promotions).Where(p => p.Promotions.Any(p => p.Id == promotion.Id)).FirstOrDefaultAsync();
                        if (userprmo != null)
                        {
                            user.Promotions.Remove(promotion);

                        }
                    }

                    // Tính điểm thưởng
                    var totalPoint = calculatePoints(seats);
                    user.PointSaving += totalPoint;
                    await _userManager.UpdateAsync(user);
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


        #endregion

        #region VNPay
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
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    // Xử lý mã giảm giá nếu có
                    var ckModel = checkoutData.CheckoutModel;
                    var promotion = await _promotionRepository.GetByCodeAsync(ckModel.PromotionCode);

                    if (promotion != null)
                    {
                        var userprmo = await _context.Users.Include(u => u.Promotions)
                            .Where(p => p.Promotions.Any(p => p.Id == promotion.Id))
                            .FirstOrDefaultAsync();

                        if (userprmo != null)
                        {
                            user.Promotions.Remove(promotion);
                        }
                    }

                    // Tính điểm thưởng
                    var seatIds = ckModel.SelectedSeats.Select(ss => Convert.ToInt32(ss.Id)).ToList();

                    var seats = await _context.Seats
                        .Include(s => s.SeatType)
                        .Where(s => seatIds.Contains(s.SeatId))
                        .ToListAsync();
                    var totalPoint = calculatePoints(seats);
                    user.PointSaving += totalPoint;
                    await _userManager.UpdateAsync(user);


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
        #endregion

        #region Other Helper Methods
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
        private long calculatePoints(List<Seat> seats)
        {
            // Giả sử mỗi ghế có 10 điểm
            long totalPoint = 0L;
            foreach (var seat in seats)
            {
                if (seat.SeatType != null)
                {
                    if (seat.SeatType.TypeDescription.Equals("regular", StringComparison.OrdinalIgnoreCase))
                    {
                        totalPoint += seat.SeatType.PointGiving;

                    }
                    else if (seat.SeatType.TypeDescription.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                    {
                        totalPoint += seat.SeatType.PointGiving;
                    }
                    else if (seat.SeatType.TypeDescription.Equals("couple", StringComparison.OrdinalIgnoreCase))
                    {
                        totalPoint += seat.SeatType.PointGiving;
                    }
                }
            }
            return totalPoint;
        }

        #endregion

        #region MoMo
        [Route("thanh-toan-momo")]
        [HttpGet]
        public async Task<IActionResult> ProcessMomoPayment()
        {
            // Get checkout data from session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["PaymentFailMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                return RedirectToAction("Login", "Account");
            }


            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var returnUrl = $"{baseUrl}/thanh-toan/momo-callback";
            var notifyUrl = $"{baseUrl}/thanh-toan/momo-ipn";
            var orderId = DateTime.Now.Ticks.ToString(); // Tạo ID đơn hàng duy nhất

            try
            {
                // Tạo yêu cầu thanh toán MoMo
                var momoRequest = new MoMoPaymentRequest
                {
                    OrderId = orderId,
                    Amount = (long)checkoutData.FinalPrice,
                    OrderInfo = $"Thanh toan ve xem phim cho {user.FullName}",
                    RedirectUrl = returnUrl,
                    IpnUrl = notifyUrl,
                    ExtraData = "",
                    Lang = "vi",
                };

                // Lưu thông tin thanh toán vào session để xử lý callback
                HttpContext.Session.SetObjectAsJson("PendingCheckoutData", checkoutData);

                // Gọi API MoMo và nhận URL thanh toán
                var momoResponse = await _momoService.CreatePaymentAsync(momoRequest);

                if (momoResponse.ErrorCode == 0) // Nếu tạo URL thành công
                {
                    // Chuyển hướng người dùng đến trang thanh toán MoMo
                    return Redirect(momoResponse.PayUrl);
                }
                else
                {
                    TempData["PaymentFailMessage"] = $"Lỗi khi tạo thanh toán MoMo: {momoResponse.Message}";
                    return RedirectToAction("PaymentFail");
                }
            }
            catch (Exception ex)
            {
                TempData["PaymentFailMessage"] = $"Lỗi khi tạo thanh toán MoMo: {ex.Message}";
                return RedirectToAction("PaymentFail");
            }


        }

        [HttpGet]
        [Route("momo-callback")]
        public async Task<IActionResult> MomoCallback([FromQuery] string partnerCode, [FromQuery] string orderId, [FromQuery] string requestId,
                    [FromQuery] long amount, [FromQuery] string orderInfo, [FromQuery] string orderType,
                    [FromQuery] long transId, [FromQuery] int resultCode, [FromQuery] string message,
                    [FromQuery] string payType, [FromQuery] string extraData, [FromQuery] string signature, [FromQuery] long responseTime)
        {
            // Kiểm tra kết quả từ MoMo
            if (resultCode != 0)
            {
                TempData["PaymentFailMessage"] = $"Lỗi thanh toán MoMo: {message}";
                return RedirectToAction("PaymentFail");
            }

            // Lấy lại thông tin từ Session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>("PendingCheckoutData");

            if (checkoutData == null)
            {
                TempData["PaymentFailMessage"] = "Không tìm thấy thông tin giao dịch để xác thực.";
                return RedirectToAction("PaymentFail");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Xác thực signature từ MoMo nếu cần
                    bool isValidSignature = await _momoService.ValidateSignature(
                                   partnerCode, requestId, orderId, orderInfo, orderType, transId, message,
                                   resultCode, payType, amount, extraData, signature, responseTime);
                    if (!isValidSignature)
                    {
                        Console.WriteLine($"[MomoCallback] Signature validation FAILED. {message}");
                        TempData["PaymentFailMessage"] = "Chữ ký không hợp lệ. Giao dịch có thể bị giả mạo.";
                        return RedirectToAction("PaymentFail");
                    }

                    // Xử lý mã giảm giá nếu có
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    // Xử lý mã giảm giá nếu có
                    var ckModel = checkoutData.CheckoutModel;
                    var promotion = await _promotionRepository.GetByCodeAsync(ckModel.PromotionCode);

                    if (promotion != null)
                    {
                        var userprmo = await _context.Users.Include(u => u.Promotions)
                            .Where(p => p.Promotions.Any(p => p.Id == promotion.Id))
                            .FirstOrDefaultAsync();

                        if (userprmo != null)
                        {
                            user.Promotions.Remove(promotion);
                        }
                    }

                    // Tính điểm thưởng
                    var seatIds = ckModel.SelectedSeats.Select(ss => Convert.ToInt32(ss.Id)).ToList();

                    var seats = await _context.Seats
                        .Include(s => s.SeatType)
                        .Where(s => seatIds.Contains(s.SeatId))
                        .ToListAsync();
                    var totalPoint = calculatePoints(seats);
                    user.PointSaving += totalPoint;
                    await _userManager.UpdateAsync(user);

                    // Tạo Receipt
                    var receipt = new Receipt
                    {
                        Date = DateTime.Now,
                        TotalPrice = (int)checkoutData.FinalPrice,
                        PaymentType = "MoMo",
                        ComboFoodId = ParseComboId(checkoutData.CheckoutModel.ComboIdAndPrice),
                        UserId = checkoutData.UserId,
                        IsPaid = true,
                        MoMoTransactionId = transId.ToString() // Lưu ID giao dịch từ MoMo nếu cần
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

                    TempData["SuccessMessage"] = "Thanh toán MoMo thành công!";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    Console.WriteLine($"[MomoCallback] Error processing transaction: {ex.ToString()}"); // Log chi tiết lỗi
                    TempData["PaymentFailMessage"] = $"Có lỗi trong quá trình xử lý giao dịch: {ex.Message}";
                    return RedirectToAction("PaymentFail");
                }
            }
        }

        [HttpPost]
        [Route("momo-ipn")]
        public async Task<IActionResult> MomoIPN([FromBody] MomoIPNRequest request)
        {
            try
            {
                // Xác thực thông tin từ MoMo IPN
                bool isValid = await _momoService.ValidateIPNRequest(request);
                if (!isValid)
                {
                    return BadRequest(new { RspCode = "99", Message = "Invalid signature" });
                }

                // Kiểm tra kết quả thanh toán
                if (request.ResultCode == 0)
                {
                    // Xử lý cập nhật trạng thái đơn hàng nếu cần
                    // Đây là nơi bạn có thể cập nhật trạng thái thanh toán trong database

                    // Trả về kết quả cho MoMo
                    return Ok(new { RspCode = "00", Message = "Success" });
                }

                return Ok(new { RspCode = "99", Message = "Payment failed" });
            }
            catch (Exception ex)
            {
                // Log lỗi
                return BadRequest(new { RspCode = "99", Message = "Internal server error" });
            }
        }


        #endregion

        #region PayPal
        [Route("thanh-toan-paypal")]
        public async Task<IActionResult> ProcessPayPalPayment()
        {
            // Get checkout data from session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["PaymentFailMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var returnUrl = $"{baseUrl}/thanh-toan/paypal-callback";
            var cancelUrl = $"{baseUrl}/thanh-toan/paypal-cancel";

            try
            {
                decimal totalPriceUSD = await ExchangeCurrencyService.ConvertCurrency((long)checkoutData.FinalPrice);
                totalPriceUSD = Math.Round(totalPriceUSD, 2);

                var paymentUrl = _payPalService.CreatePayment(
                    totalPriceUSD,
                    "USD", 
                    returnUrl,
                    cancelUrl
                );

                HttpContext.Session.SetObjectAsJson("PendingCheckoutData", checkoutData);

                // Chuyển hướng người dùng đến trang thanh toán PayPal
                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                TempData["PaymentFailMessage"] = $"Lỗi khi tạo thanh toán PayPal: {ex.Message}";
                return RedirectToAction("Add", new { model = checkoutData.CheckoutModel });
            }

        }

        [HttpGet]
        [Route("paypal-callback")]
        public async Task<IActionResult> PayPalCallback([FromQuery] string PaymentId, [FromQuery] string token, [FromQuery] string PayerID)
        {
            if (string.IsNullOrEmpty(PaymentId) || string.IsNullOrEmpty(PayerID))
            {
                TempData["PaymentFailMessage"] = "Thông tin thanh toán không hợp lệ.";
                return RedirectToAction("PaymentFail");
            }

            // Lấy lại thông tin từ Session
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>("PendingCheckoutData");

            if (checkoutData == null)
            {
                TempData["PaymentFailMessage"] = "Không tìm thấy thông tin giao dịch để xác thực.";
                return RedirectToAction("PaymentFail");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Thực hiện thanh toán với PayPal
                    var executedPayment = _payPalService.ExecutePayment(PaymentId, PayerID);

                    // Kiểm tra trạng thái thanh toán
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        TempData["PaymentFailMessage"] = "Thanh toán không được chấp nhận.";
                        return RedirectToAction("PaymentFail");
                    }

                    // Xử lý mã giảm giá nếu có
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    // Xử lý mã giảm giá nếu có
                    var ckModel = checkoutData.CheckoutModel;
                    var promotion = await _promotionRepository.GetByCodeAsync(ckModel.PromotionCode);

                    if (promotion != null)
                    {
                        var userprmo = await _context.Users.Include(u => u.Promotions)
                            .Where(p => p.Promotions.Any(p => p.Id == promotion.Id))
                            .FirstOrDefaultAsync();

                        if (userprmo != null)
                        {
                            user.Promotions.Remove(promotion);
                        }
                    }

                    // Tính điểm thưởng
                    var seatIds = ckModel.SelectedSeats.Select(ss => Convert.ToInt32(ss.Id)).ToList();

                    var seats = await _context.Seats
                        .Include(s => s.SeatType)
                        .Where(s => seatIds.Contains(s.SeatId))
                        .ToListAsync();
                    var totalPoint = calculatePoints(seats);
                    user.PointSaving += totalPoint;
                    await _userManager.UpdateAsync(user);

                    // Tạo Receipt
                    var receipt = new Receipt
                    {
                        Date = DateTime.Now,
                        TotalPrice = (int)checkoutData.FinalPrice,
                        PaymentType = "PayPal",
                        ComboFoodId = ParseComboId(checkoutData.CheckoutModel.ComboIdAndPrice),
                        UserId = checkoutData.UserId,
                        IsPaid = true
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

                    TempData["SuccessMessage"] = "Thanh toán PayPal thành công!";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error Type: {ex.GetType().Name}");
                    Console.WriteLine($"Error Message: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }

                    TempData["PaymentFailMessage"] = $"Có lỗi trong quá trình xử lý giao dịch: {ex.Message}";
                    return RedirectToAction("PaymentFail");
                }
            }
        }

        [HttpGet]
        [Route("paypal-cancel")]
        public IActionResult PayPalCancel()
        {
            TempData["PaymentFailMessage"] = "Bạn đã hủy thanh toán qua PayPal.";
            return RedirectToAction("PaymentFail");
        }

        #endregion

        #region Announcement
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

        #endregion

    }

    // Class to store checkout data in session
    public class CheckoutSessionData
    {
        public CheckoutSummaryVM CheckoutModel { get; set; }
        public decimal FinalPrice { get; set; }
        public string UserId { get; set; }
    }
}