using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.GlobalServices;
using school_major_project.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/hoa-don")]
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly PrintingTicket _printingTicket;

        public ReceiptsController(ApplicationDbContext context, IReceiptRepository receipt, PrintingTicket printingTicket)
        {
            _context = context;
            _receiptRepository = receipt;
            _printingTicket = printingTicket;
        }


        [Route("")]
        public async Task<IActionResult> Index()
        {
            var receipts = await _receiptRepository.GetAllAsync();
            return View(receipts);
        }


        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {


            var receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            return Json(new { Receipt = receipt }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64
            });
        }

        [HttpGet]
        [Route("in-ve/{id}")]
        public async Task<IActionResult> GenerateTicketPDFs(int id)
        {
            try
            {
                var receipt = await _receiptRepository.GetByIdAsync(id);
                if (receipt == null)
                {
                    return NotFound($"Booking with ID {id} not found.");
                }

                string templatePath = "admin/templates/template-ticket.pdf";
                string outputDir = "admin/tickets";

                List<string> relativeFilePaths = _printingTicket.GeneratePdfs(receipt, templatePath, outputDir);


                Console.WriteLine($"PDFs generated successfully: {string.Join(", ", relativeFilePaths)}");

                List<string> publicUrls = relativeFilePaths
                    .Select(relativeFilePath => Url.Content($"~/{relativeFilePath.Replace('\\', '/')}"))
                    .ToList();


                Console.WriteLine($"Public URLs generated: {string.Join(", ", publicUrls)}");


                return Ok(publicUrls);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in GenerateTicketPDFs: {ex.ToString()}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"Error: {ex.Message}" });
            }
        }

    }
}
