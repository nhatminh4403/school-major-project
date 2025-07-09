using school_major_project.Models;
using school_major_project.ViewModel;
using System.Threading.Tasks;
using school_major_project.Models;
using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ITicketService
    {
        Task<Receipt> CreateReceiptAndDetailsAsync(CheckoutSummaryVM model, decimal finalPrice, string userId, bool sendEmail = true);
        Task SendTicketEmailAsync(User user, Receipt receipt, CheckoutSummaryVM model, decimal finalPrice);
    }
} 