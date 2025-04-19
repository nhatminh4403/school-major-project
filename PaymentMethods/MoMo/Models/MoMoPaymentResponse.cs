namespace school_major_project.PaymentMethods.MoMo.Models
{
    public class MoMoPaymentResponse
    {
        public string RequestId { get; set; }
        public int ErrorCode { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
        public string PayUrl { get; set; }
        public string Signature { get; set; }
    }
}
