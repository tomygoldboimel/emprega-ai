using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmpregaAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AudioController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("sintetizar")]
        public async Task<IActionResult> SintetizarVoz([FromQuery] string texto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(texto))
                {
                    return BadRequest("Texto não pode ser vazio");
                }
                var textoLimitado = texto.Length > 200
                    ? texto.Substring(0, 200)
                    : texto;

                var textoEncoded = Uri.EscapeDataString(textoLimitado);
                var url = $"https://translate.google.com/translate_tts?ie=UTF-8&tl=pt-BR&client=tw-ob&q={textoEncoded}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var audioBytes = await response.Content.ReadAsByteArrayAsync();
                    return File(audioBytes, "audio/mpeg", "audio.mp3");
                }

                return StatusCode((int)response.StatusCode, "Erro ao obter áudio");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}