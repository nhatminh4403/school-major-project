using PayPal.Api;

namespace school_major_project.PaymentMethods.PayPal
{
    public interface IPayPalService
    {
        string CreatePayment(decimal amount, string currency, string returnUrl, string cancelUrl);
        Payment ExecutePayment(string paymentId, string payerId);
    }
}
