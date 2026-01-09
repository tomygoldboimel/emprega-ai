namespace EmpregaAI.Services.Interfaces
{
    public interface IExtratorService
    {
        Task<string> ExtractTextFromPdfAsync(Stream pdfStream);
        Task<string> ExtractTextFromDocxAsync(Stream docxStream);
    }
}
