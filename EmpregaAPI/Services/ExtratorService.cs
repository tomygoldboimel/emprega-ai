using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using System.Reflection.PortableExecutable;
using EmpregaAI.Services.Interfaces;

namespace EmpregaAI.Services
{

    public class ExtratorService : IExtratorService
    {
        public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
        {
            return await Task.Run(() =>
            {
                using var pdfReader = new PdfReader(pdfStream);
                using var pdfDocument = new PdfDocument(pdfReader);

                var text = new StringBuilder();

                for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    string pageText = PdfTextExtractor.GetTextFromPage(
                        pdfDocument.GetPage(page),
                        strategy
                    );
                    text.AppendLine(pageText);
                }

                return text.ToString();
            });
        }

        public async Task<string> ExtractTextFromDocxAsync(Stream docxStream)
        {
            return await Task.Run(() =>
            {
                var text = new StringBuilder();

                using var doc = WordprocessingDocument.Open(docxStream, false);
                var body = doc.MainDocumentPart?.Document.Body;

                if (body != null)
                {
                    foreach (var paragraph in body.Descendants<Paragraph>())
                    {
                        text.AppendLine(paragraph.InnerText);
                    }
                }

                return text.ToString();
            });
        }
    }
}