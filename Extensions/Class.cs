using Microsoft.AspNetCore.Mvc;
using school_major_project.ViewModel;
using System.Text.Json;

namespace school_major_project.Extensions
{
    public class Class
    {
     /*   public IActionResult ProcessPayPalPayment(int receiptId)
        {
            // Similar implementation for PayPal
            // After successful payment processing:
            TempData["SuccessMessage"] = "Thanh toán PayPal thành công!";
            return RedirectToAction("History", "User");
        }
        [HttpPost]
        [Route("thanh-toan-momo")]
        public async Task<IActionResult> ProcessMomoPayment()
        {
            // TODO: gọi API MOMO, kiểm tra kết quả
            bool paymentSuccess = *//* gọi MOMO, nhận kết quả *//*true;

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
                return View("Add");
            }
        }*/

    }
}
