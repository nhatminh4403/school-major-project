using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using school_major_project.Configuration;
using school_major_project.DataAccess;
using school_major_project.HelperClass;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace school_major_project.ModelServices
{
    public class TicketService : ITicketService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailsRepository _receiptDetailsRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly EmailSettings _settings;


        public TicketService(IOptions<EmailSettings> options, IReceiptRepository receiptRepository, IReceiptDetailsRepository receiptDetailsRepository, IEmailService emailService, UserManager<User> userManager, ApplicationDbContext context)
        {
            _receiptRepository = receiptRepository;
            _receiptDetailsRepository = receiptDetailsRepository;
            _emailService = emailService;
            _userManager = userManager;
            _context = context;
            _settings = options.Value;

        }

        public async Task<Receipt> CreateReceiptAndDetailsAsync(CheckoutSummaryVM model, decimal finalPrice, string userId, bool sendEmail = true)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (string.IsNullOrEmpty(user?.Email))
                throw new Exception("Email người dùng không hợp lệ.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create receipt and details
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
                    await transaction.CommitAsync();

                    // Gửi email nếu được yêu cầu
                    if (sendEmail)
                    {
                        await SendTicketEmailAsync(user, receipt, model, finalPrice);
                    }

                    return receipt;                    
                }
                catch (Exception ex)
                {
                    // Rollback transaction on error
                    await transaction.RollbackAsync();
                    throw new Exception("Lỗi khi tạo hóa đơn và gửi email: " + ex.Message);
                }
            }
        }

        public async Task SendTicketEmailAsync(User user, Receipt receipt, CheckoutSummaryVM model, decimal finalPrice)
        {
            if (string.IsNullOrEmpty(user.Email)) return;
            string qrContent = $"BOOKING-{receipt.Id}";
            byte[] qrImage = QrCodeHelper.GenerateQrCode(qrContent);
            string emailBody = $@"<p>Cảm ơn bạn đã đặt vé tại hệ thống!</p>
                <p>Thông tin vé:</p>
                <ul>
                <li>Phim: {model.FilmTitle}</li>
                <li>Rạp: {model.CinemaName}</li>
                <li>Phòng: {model.RoomName}</li>
                <li>Ghế: {string.Join(", ", model.SelectedSeats.Select(s => s.Symbol))}</li>
                <li>Thời gian: {model.StartTime:dd/MM/yyyy HH:mm}</li>
                <li>Tổng tiền: {finalPrice:N0} VNĐ</li>
                </ul>
                <p>Vui lòng xuất trình mã QR đính kèm khi đến rạp.</p>";
            await _emailService.SendEmailAsync(_settings.Username, "Xác nhận đặt vé thành công", emailBody, qrImage, "qrcode.png", true);
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