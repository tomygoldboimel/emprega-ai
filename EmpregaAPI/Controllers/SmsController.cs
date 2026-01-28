using EmpregaAI.DTOs;
using EmpregaAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly VonageSmsService _smsService;

    public AuthController(VonageSmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> EnviarCodigo([FromBody] EnviarCodigoRequest request)
    {
        await _smsService.EnviarCodigoAsync(request.Telefone);
        return Ok("Ok");
    }

    [HttpPost("verify-code")]
    public IActionResult VerificarCodigo([FromBody] VerificarCodigoRequest request)
    {
        // Apenas valida se o código no cache coincide com o informado
        var valido = _smsService.VerificarCodigo(
            request.Telefone,
            request.Codigo
        );

        if (!valido)
            return BadRequest("Código inválido ou expirado");

        // Retorna apenas sucesso. O Frontend receberá isso e saberá que pode prosseguir.
        return Ok(new { message = "Código verificado com sucesso" });
    }
}

