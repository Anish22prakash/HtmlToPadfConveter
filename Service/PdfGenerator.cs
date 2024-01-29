using DinkToPdf.Contracts;
using DinkToPdf;
using ceTe.DynamicPDF.Conversion;


namespace HtmlToPdfConverterWeb.Service
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConverter _converter;

        public PdfGenerator(IConverter converter)
        {
            this._converter = converter;
        }

        public byte[] GeneratorPdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings { Top = 1, Bottom = 1, Left = 1, Right = 1 },
                DocumentTitle = "Zyod"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 8, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                FooterSettings = { FontSize = 8, Line = true, Right = "© " + DateTime.Now.Year }
            };

            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(document);
        }
    }
}
