using Microsoft.AspNetCore.Authorization;
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
using school_major_project.HelperClass;

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
        private readonly IMoMoService _momoService;
        private readonly IEmailService _emailService;
        private readonly ITicketService _ticketService;
        // Session key constants
        private const string CheckoutSessionKey = "CheckoutData";
        public PurchaseController(ApplicationDbContext context, SignInManager<User> signInManager, IPromotionRepository promotionRepository,
            UserManager<User> userManager, IFoodRepository foodRepository, IReceiptRepository receiptRepository,
            IReceiptDetailsRepository receiptDetailsRepository,
            IVnPayService vnPayService, IPayPalService payPalService, IMoMoService momoService, IEmailService emailService, ITicketService ticketService) : base(context)
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
            _emailService = emailService;
            _ticketService = ticketService;
        }
        #region Summary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string seatSymbol, decimal totalPrice, DateTime startTime, string filmTitle,
                                                string poster, List<string> category, string cinemaName,
                                                string cinemaAddress, string roomName, int scheduleId)
        {
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

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                return RedirectToAction("Login", "Account");
            }

            decimal finalPrice = CalculateFinalPrice(model);

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
                    var receipt = await _ticketService.CreateReceiptAndDetailsAsync(model, finalPrice, userId);
                    await _ticketService.SendTicketEmailAsync(await _userManager.GetUserAsync(User), receipt, model, finalPrice);
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
            var checkoutData = HttpContext.Session.GetObjectFromJson<CheckoutSessionData>(CheckoutSessionKey);

            if (checkoutData == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.GetUserAsync(User);

            var vnPayModel = new VnPaymentRequestModel
            {
                Price = (double)checkoutData.FinalPrice,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán vé xem phim cho {user.FullName}",
                FullName = user.FullName,
                OrderId = new Random().Next(1000, 10000)
            };

            HttpContext.Session.SetObjectAsJson("PendingCheckoutData", checkoutData);

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
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    var ckModel = checkoutData.CheckoutModel;
                    var receipt = await _ticketService.CreateReceiptAndDetailsAsync(ckModel, checkoutData.FinalPrice, checkoutData.UserId);
                    await _ticketService.SendTicketEmailAsync(user, receipt, ckModel, checkoutData.FinalPrice);

                    await transaction.CommitAsync();

                    HttpContext.Session.Remove("PendingCheckoutData");

                    TempData["SuccessMessage"] = "Thanh toán VNPay thành công!";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = "Có lỗi trong quá trình xử lý giao dịch.";
                    return RedirectToAction("PaymentFail");
                }
            }
        }
        #endregion

        #region Other Helper Methods
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

        private async Task<Receipt> SaveReceiptAsync(CheckoutSummaryVM model, decimal finalPrice, string userId)
        {
            var receipt = new Receipt
            {
                Date = DateTime.Now,
                TotalPrice = (int)finalPrice,
                PaymentType = model.PaymentMethod,
                ComboFoodId = ParseComboId(model.ComboIdAndPrice),
                UserId = userId,
                IsPaid = model.PaymentMethod != "Cash" 
            };
            await _receiptRepository.AddAsync(receipt);

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
            if (resultCode != 0)
            {
                TempData["PaymentFailMessage"] = $"Lỗi thanh toán MoMo: {message}";
                return RedirectToAction("PaymentFail");
            }

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
                    bool isValidSignature = await _momoService.ValidateSignature(
                                   partnerCode, requestId, orderId, orderInfo, orderType, transId, message,
                                   resultCode, payType, amount, extraData, signature, responseTime);
                    if (!isValidSignature)
                    {
                        Console.WriteLine($"[MomoCallback] Signature validation FAILED. {message}");
                        TempData["PaymentFailMessage"] = "Chữ ký không hợp lệ. Giao dịch có thể bị giả mạo.";
                        return RedirectToAction("PaymentFail");
                    }

                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    var ckModel = checkoutData.CheckoutModel;
                    var receipt = await _ticketService.CreateReceiptAndDetailsAsync(ckModel, checkoutData.FinalPrice, checkoutData.UserId);
                    await _ticketService.SendTicketEmailAsync(user, receipt, ckModel, checkoutData.FinalPrice);

                    await transaction.CommitAsync();

                    HttpContext.Session.Remove("PendingCheckoutData");

                    TempData["SuccessMessage"] = "Thanh toán MoMo thành công!";
                    return RedirectToAction("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"[MomoCallback] Error processing transaction: {ex.ToString()}"); 
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
                bool isValid = await _momoService.ValidateIPNRequest(request);
                if (!isValid)
                {
                    return BadRequest(new { RspCode = "99", Message = "Invalid signature" });
                }

                if (request.ResultCode == 0)
                {

                    return Ok(new { RspCode = "00", Message = "Success" });
                }

                return Ok(new { RspCode = "99", Message = "Payment failed" });
            }
            catch (Exception ex)
            {
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

                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["LoginMessage"] = "Vui lòng đăng nhập để thực hiện thanh toán.";
                        return RedirectToAction("Login", "Account");
                    }

                    var ckModel = checkoutData.CheckoutModel;
                    var receipt = await _ticketService.CreateReceiptAndDetailsAsync(ckModel, checkoutData.FinalPrice, checkoutData.UserId);
                    await _ticketService.SendTicketEmailAsync(user, receipt, ckModel, checkoutData.FinalPrice);

                    await transaction.CommitAsync();

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

    public class CheckoutSessionData
    {
        public CheckoutSummaryVM CheckoutModel { get; set; }
        public decimal FinalPrice { get; set; }
        public string UserId { get; set; }
    }
}