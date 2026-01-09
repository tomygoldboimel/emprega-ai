using EmpregaAI.Models.Resume;
using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace EmpregaAI.Controllers
{
    [ApiController]
    [Route("api/resume")]
    public class CurriculoUploadController : ControllerBase
    {
        private readonly IExtratorService _textExtractor;
        private readonly IProcessadorGroqService _processingService;
        private readonly ILogger<CurriculoUpload> _logger;
        private const long MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB

        public CurriculoUploadController(
            IExtratorService textExtractor,
            IProcessadorGroqService processingService,
            ILogger<CurriculoUpload> logger)
        {
            _textExtractor = textExtractor;
            _processingService = processingService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(MAX_FILE_SIZE)]
        public async Task<ActionResult<CurriculoUpload>> UploadResume(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "Nenhum arquivo foi enviado." });
                }

                if (file.Length > MAX_FILE_SIZE)
                {
                    return BadRequest(new { message = "Arquivo muito grande. Máximo: 10MB." });
                }

                var allowedExtensions = new[] { ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Formato inválido. Apenas PDF e DOCX são permitidos." });
                }

                _logger.LogInformation($"Processando arquivo: {file.FileName}");

                string extractedText;
                using (var stream = file.OpenReadStream())
                {
                    extractedText = fileExtension switch
                    {
                        ".pdf" => await _textExtractor.ExtractTextFromPdfAsync(stream),
                        ".docx" => await _textExtractor.ExtractTextFromDocxAsync(stream),
                        _ => throw new InvalidOperationException("Formato não suportado")
                    };
                }

                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    return BadRequest(new { message = "Não foi possível extrair texto do arquivo." });
                }

                _logger.LogInformation($"Texto extraído com sucesso. Tamanho: {extractedText.Length} caracteres");

                var processedData = await _processingService.ProcessResumeTextAsync(extractedText);

                return Ok(processedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar currículo");
                return StatusCode(500, new { message = "Erro ao processar currículo. Tente novamente." });
            }
        }
    }
}