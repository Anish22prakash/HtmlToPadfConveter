using ceTe.DynamicPDF.HtmlConverter;
using DinkToPdf;
using DinkToPdf.Contracts;
using HtmlToPdfConverterWeb.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.Net;

namespace HtmlToPdfConverterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfsController : ControllerBase
    {
        private readonly IPdfGenerator _pdfGenerator;

        public PdfsController(IPdfGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }


        [HttpPost("ConvertHtmlToPdf")]
        public IActionResult ConvertHtmlToPdf([FromBody] string htmlContent)
        {
       //this method is taking 15-20sec to convert the html to pdf and save it to local(html 50page with img).
            try
            {
                if (string.IsNullOrWhiteSpace(htmlContent))
                {
                    return BadRequest("HTML content is null or empty.");
                }
                ConversionOptions options = new ConversionOptions(PageSize.A4, PageOrientation.Portrait, 50.0f);

                options.Author = "Zyod";
                options.Title = "Zyodweb";

                options.Header = "<div style=\"text-align:center;display:inline-block;width:100%;font-size:12px;\">" +
                    "<span class=\"date\"></span></div>";
                options.Footer = "<div style=\"text-align:center;display:inline-block;width:100%;font-size:12px;\">" +
                    "Page <span class=\"pageNumber\"></span> of <span class=\"totalPages\"></span></div>";

                Converter.Convert(new Uri(htmlContent), @"C:\HARD DRIVE\Zyod.pdf", options);

                return Ok("HTML converted to PDF successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

      
        [HttpPost("ConvertHTMLtoPdfMethodTwo")]
        public IActionResult ConvertHTMLtoPdfMethodTwo(string htmlContent)
        {
            //this method is taking 25-30sec to convert the html to pdf and save it to local(html 50page with img).
            try
            {
                if (string.IsNullOrWhiteSpace(htmlContent))
                {
                    return BadRequest("HTML content is null or empty.");
                }
                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument docs = converter.ConvertUrl(htmlContent);

                docs.Save($@"{AppDomain.CurrentDomain.BaseDirectory}\Zyod.pdf");
                docs.Close();

                return Ok("HTML converted to PDF successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("ConvertHtmlToPdfDink")]
        public async Task<IActionResult> ConvertHtmlToPdfDink(string url)
        {
            //this method is taking 10 sec but only able to convert images not text
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return BadRequest("URL is required.");
                }

                var htmlContent = await GetHtmlContent(url);

                if (htmlContent == null)
                {
                    return BadRequest("Unable to fetch HTML content from the specified URL.");
                }

                byte[] pdfBytes = _pdfGenerator.GeneratorPdf(htmlContent);

                return File(pdfBytes, "application/pdf", "generated.pdf");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        private async Task<string> GetHtmlContent(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    return await client.DownloadStringTaskAsync(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
