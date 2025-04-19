using school_major_project.PaymentMethods.MoMo.Models;

namespace school_major_project.PaymentMethods.MoMo.Services
{
    public interface IMoMoService
    {

        Task<MoMoPaymentResponse> CreatePaymentAsync(MoMoPaymentRequest request);
        Task<bool> ValidateSignature(string partnerCode, string requestId, string orderId, string orderInfo,
                                    string orderType, long transId, string message,
                                    int resultCode, string payType, long amount,
                                    string extraData, string signatureFromCallback, long responseTime);
        Task<bool> ValidateIPNRequest(MomoIPNRequest request);
    }
}
