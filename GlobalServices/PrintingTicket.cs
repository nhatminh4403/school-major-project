using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using school_major_project.HelperClass;
using school_major_project.Models;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;

namespace school_major_project.GlobalServices
{
    public class PrintingTicket
    {
        private const int BARCODE_WIDTH = 200;
        private const int BARCODE_HEIGHT = 40;
        private const int MARGIN = 20;
        private const float FONT_SIZE = 9;
        private const float TEXT_MARGIN = 5;
        private readonly Random _random = new Random();
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PrintingTicket(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        private byte[] BitmapToByteArray(Bitmap image)
        {
            using var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        private string GenerateSerialNumber(string seatSymbol)
        {

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            string timeComponent = (timestamp % 10000).ToString("D4");


            string cleanedSeatSymbol = System.Text.RegularExpressions.Regex.Replace(seatSymbol, "[^A-Za-z0-9]", "");
            string seatComponent;
            if (cleanedSeatSymbol.Length >= 2)
            {
                seatComponent = cleanedSeatSymbol.Substring(cleanedSeatSymbol.Length - 2);
            }
            else
            {
                seatComponent = cleanedSeatSymbol.PadLeft(2, '0');
            }


            string randomComponent = _random.Next(0, 100).ToString("D2");


            return $"{timeComponent}-{seatComponent}-{randomComponent}";
        }
        private string GetAbsolutePath(string relativePath)
        {


            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.Substring(1);
            }


            return System.IO.Path.Combine(_hostingEnvironment.WebRootPath, relativePath);


        }
        private PdfFont LoadFont(PdfDocument document)
        {

            string montserratPathRelative = "admin/fonts/Montserrat/Montserrat-Regular.otf";
            string montserratPathAbsolute = GetAbsolutePath(montserratPathRelative);

            try
            {
                if (File.Exists(montserratPathAbsolute))
                {
                    Console.WriteLine("Montserrat font found");

                    return PdfFontFactory.CreateFont(montserratPathAbsolute, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                }
                Console.WriteLine($"Montserrat font not found at: {montserratPathAbsolute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Montserrat font: {ex.Message}");

            }


            string robotoPathRelative = "admin/fonts/RobotoSlab/static/RobotoSlab-Regular.ttf";
            string robotoPathAbsolute = GetAbsolutePath(robotoPathRelative);
            try
            {
                if (File.Exists(robotoPathAbsolute))
                {
                    Console.WriteLine("RobotoSlab font found");
                    return PdfFontFactory.CreateFont(robotoPathAbsolute, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                }
                Console.WriteLine($"RobotoSlab font not found at: {robotoPathAbsolute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading RobotoSlab font: {ex.Message}");

            }


            string timesPathRelative = "admin/fonts/Times New Roman/times new roman.ttf";
            string timesPathAbsolute = GetAbsolutePath(timesPathRelative);
            try
            {
                if (File.Exists(timesPathAbsolute))
                {
                    Console.WriteLine("Times New Roman font found");
                    return PdfFontFactory.CreateFont(timesPathAbsolute, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                }
                Console.WriteLine($"Times New Roman font not found at: {timesPathAbsolute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Times New Roman font: {ex.Message}");
            }

            Console.WriteLine("Using built-in Helvetica as last resort font.");
            return PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        }
        private void SetFieldFont(PdfAcroForm acroForm, PdfFont font)
        {
            foreach (var fieldName in acroForm.GetAllFormFields().Keys)
            {
                PdfFormField field = acroForm.GetField(fieldName);
                if (field != null)
                {


                    field.SetFont(font).SetFontSize(12);





                    field.SetColor(ColorConstants.BLACK);
                }
            }


        }
        public byte[] FillPdf(string templatePathAbsolute, Receipt receipt, ReceiptDetail detail)
        {
            using var outputMemoryStream = new MemoryStream();

            using (var pdfReader = new PdfReader(templatePathAbsolute))
            using (var pdfWriter = new PdfWriter(outputMemoryStream))
            using (var document = new PdfDocument(pdfReader, pdfWriter))
            {
                PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(document, true);
                PdfFont customFont = LoadFont(document);
                SetFieldFont(acroForm, customFont);

                string seatSymbol = detail.SeatName;
                if (!string.IsNullOrEmpty(seatSymbol))
                {
                    BarcodeWriterPixelData barcodeWriter = new BarcodeWriterPixelData { /* ... */ Format = BarcodeFormat.CODE_128, Options = new EncodingOptions { Height = (int)BARCODE_HEIGHT * 3, Width = (int)BARCODE_WIDTH * 3, PureBarcode = true, Margin = 0 } };
                    string serialNumber = GenerateSerialNumber(seatSymbol);
                    var pixelData = barcodeWriter.Write(seatSymbol.ToLowerInvariant());

                    Bitmap barcodeBitmap = null;
                    var handle = System.Runtime.InteropServices.GCHandle.Alloc(pixelData.Pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
                    try
                    { /* ... create bitmap ... */
                        IntPtr pointer = handle.AddrOfPinnedObject();
                        barcodeBitmap = new Bitmap(pixelData.Width, pixelData.Height, pixelData.Width * 4, PixelFormat.Format32bppPArgb, pointer);
                    }
                    finally { if (handle.IsAllocated) handle.Free(); }

                    if (barcodeBitmap != null)
                    {
                        byte[] barcodeBytes = BitmapToByteArray(barcodeBitmap);
                        ImageData barcodeImageData = ImageDataFactory.Create(barcodeBytes);

                        PdfPage page = document.GetPage(1);
                        iText.Kernel.Geom.Rectangle pageSize = page.GetPageSize();
                        float x = pageSize.GetRight() - BARCODE_WIDTH - MARGIN;
                        float y = MARGIN;
                        float imageY = y + TEXT_MARGIN + FONT_SIZE;
                        PdfCanvas canvas = new PdfCanvas(page);
                        canvas.AddImageFittedIntoRectangle(barcodeImageData, new iText.Kernel.Geom.Rectangle(x, imageY, BARCODE_WIDTH, BARCODE_HEIGHT), false);

                        PdfFont textFont = PdfFontFactory.CreateFont(StandardFonts.COURIER);
                        float textWidth = textFont.GetWidth(serialNumber, FONT_SIZE);
                        float textX = x + (BARCODE_WIDTH - textWidth) / 2;
                        canvas.BeginText().SetFontAndSize(textFont, FONT_SIZE).SetColor(ColorConstants.BLACK, true).MoveText(textX, y).ShowText(serialNumber).EndText();
                        barcodeBitmap.Dispose();
                    }
                }
                else
                {
                    Console.WriteLine("Seat symbol is missing for ReceiptDetail ID: {ReceiptDetailId}. Skipping barcode generation.", detail.Id);
                }



                FillFormFields(acroForm, receipt, detail, customFont);

            }

            return outputMemoryStream.ToArray();

        }





        private string SeparateRoomName(string roomName)
        {
            if (string.IsNullOrEmpty(roomName)) return "";
            string[] firstPartOfName = roomName.Split('-');
            if (firstPartOfName.Length == 0) return "";
            string[] getRoomNumberParts = firstPartOfName[0].Trim().Split(' ');

            return getRoomNumberParts.LastOrDefault(StringHelper.IsNumeric) ?? "N/A";
        }

        private void FillFormFields(PdfAcroForm acroForm, Receipt receipt, ReceiptDetail detail, PdfFont font)
        {

            SetField(acroForm, "movie_name", StringHelper.Capitalize(StringHelper.RemoveDiacritics(detail.FilmName)), font);
            SetField(acroForm, "location1", StringHelper.Capitalize(StringHelper.RemoveDiacritics(detail.CinemaName)), font);
            SetField(acroForm, "location2", StringHelper.Capitalize(StringHelper.RemoveDiacritics(detail.CinemaName)), font);
            SetField(acroForm, "theatre_name", "BA ANH EM", font);
            SetField(acroForm, "ticket_id1", detail.Id.ToString(), font);
            SetField(acroForm, "ticket_id2", detail.Id.ToString(), font);

            string roomNumber = SeparateRoomName(detail.RoomName);
            if (string.IsNullOrEmpty(roomNumber) || roomNumber == "N/A")
            {
                roomNumber = StringHelper.RemoveDiacritics(detail.RoomName);
            }
            SetField(acroForm, "room1", roomNumber, font);
            SetField(acroForm, "room2", roomNumber, font);



            string formattedDate = detail.StartTime?.ToString("dd-MM-yyyy HH:mm") ?? string.Empty;
            SetField(acroForm, "date_booked1", formattedDate, font);
            SetField(acroForm, "date_booked2", formattedDate, font);

            SetField(acroForm, "seat_id1", detail.SeatName, font);
            SetField(acroForm, "seat_id2", detail.SeatName, font);

        }


        private void SetField(PdfAcroForm acroForm, string fieldName, string value, PdfFont font)
        {
            PdfFormField field = acroForm.GetField(fieldName);
            if (field != null)
            {
                field.SetFont(font);
                field.SetValue(value ?? "");

            }
            else
            {
                Console.WriteLine($"Warning: Field '{fieldName}' not found in PDF template.");
            }
        }


        public List<string> GeneratePdfs(Receipt receipt, string templatePathRelative, string outputDirectoryRelative)
        {
            List<string> pdfFilePaths = new List<string>();
            string templatePathAbsolute = GetAbsolutePath(templatePathRelative);
            string outputDirAbsolute = GetAbsolutePath(outputDirectoryRelative);

            if (!File.Exists(templatePathAbsolute))
            {
                throw new FileNotFoundException("Ticket template PDF not found.", templatePathAbsolute);
            }
            if (!Directory.Exists(outputDirAbsolute)) { try { Directory.CreateDirectory(outputDirAbsolute); } catch (Exception ex) { /* ... log error and throw ... */ throw new IOException($"Could not create output directory: {outputDirAbsolute}", ex); } }


            try
            {


                if (receipt.ReceiptDetails == null || !receipt.ReceiptDetails.Any())
                { /* ... log warning, return empty list ... */
                    return pdfFilePaths;
                }


                foreach (ReceiptDetail detail in receipt.ReceiptDetails)
                {
                    if (detail.Seat == null || string.IsNullOrEmpty(detail.SeatName))
                    {
                        continue;
                    }



                    byte[] pdfBytes = FillPdf(templatePathAbsolute, receipt, detail);


                    string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    string safeSeatSymbol = System.Text.RegularExpressions.Regex.Replace(detail.SeatName, "[^A-Za-z0-9_-]", "_");
                    string fileName = $"ticket_{receipt.Id}_{safeSeatSymbol}_{timestamp}.pdf";
                    string filePathAbsolute = System.IO.Path.Combine(outputDirAbsolute, fileName);

                    File.WriteAllBytes(filePathAbsolute, pdfBytes);

                    string relativeFilePath = System.IO.Path.Combine(outputDirectoryRelative, fileName).Replace('\\', '/');
                    pdfFilePaths.Add(relativeFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to generate PDF tickets for Receipt ID {receipt.Id}. Error: {ex.Message}", ex);
            }

            return pdfFilePaths;
        }
    }
}
