using EmpregaAI.Models;
using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificacaoController : ControllerBase
{
    private readonly ICertificacaoService _CertificacaoService;

    public CertificacaoController(ICertificacaoService CertificacaoService)
    {
        _CertificacaoService = CertificacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarCertificacao([FromBody] Certificacao certificacao)
    {
        try
        {
            var atualizado = await _CertificacaoService.AdicionaCertificacao(certificacao);

            if (atualizado == null)
            {
                return NotFound(new { message = "Certificação não encontrada" });
            }
            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "DataConclusao_Futura")
            {
                return BadRequest(new { code = "DataConclusao_Futura", message = "A data de conclusão não pode ser futura." });
            }

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro Interno ao atualizar Certificação: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListaCertificacaos()
    {
        var Certificacaos = await _CertificacaoService.ListarCertificacoes();
        return Ok(Certificacaos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarCertificacaoPorId(Guid id)
    {
        var Certificacao = await _CertificacaoService.ListarCertificacaoPorID(id);
        if (Certificacao == null)
        {
            return NotFound(new { message = "Certificação não encontrada" });
        }
        return Ok(Certificacao);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarCertificacao([FromBody] Certificacao certificacao)
    {
        try
        {
            var atualizado = await _CertificacaoService.AtualizarCertificacao(certificacao);

            if (atualizado == null)
            {
                return NotFound(new { message = "Certificação não encontrada" });
            }
            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "DataConclusao_Futura")
            {
                return BadRequest(new { code = "DataConclusao_Futura", message = "A data de conclusão não pode ser futura." });
            }

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro Interno ao atualizar Certificação: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpPut("Deletar/{idCertificacao}")]
    public async Task<IActionResult> ExcluirCertificacao(Guid idCertificacao)
    {
        var excluido = await _CertificacaoService.ExcluirCertificacao(idCertificacao);
        if (excluido == null)
        {
            return NotFound(new { message = "Certificação não encontrada" });
        }
        return Ok(excluido);
    }

    [HttpGet("CertificacaoPorCurriculo/{curriculoId}")]
    public async Task<IActionResult> ListarCertificacoesPorCurriculo(Guid curriculoId)
    {
        var certificacoes = await _CertificacaoService.ListarCertificacaoPorCurriculoId(curriculoId);
        return Ok(certificacoes);
    }
}