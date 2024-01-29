namespace HtmlToPdfConverterWeb.Service
{
    public interface IPdfGenerator
    {
        public byte[] GeneratorPdf(string htmlContent);
    }
}
