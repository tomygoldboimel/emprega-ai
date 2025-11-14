using EmpregaAI.Models;
using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormacaoController : ControllerBase
{
    private readonly IFormacaoService _FormacaoService;

    public FormacaoController(IFormacaoService FormacaoService)
    {
        _FormacaoService = FormacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionaFormacao([FromBody] Formacao formacao)
    {
        try
        {
            var novaFormacao = await _FormacaoService.AdicionaFormacao(formacao);

            if (novaFormacao == null)
            {
                return BadRequest(new { message = "Não foi possível adicionar a formação." });
            }

            return Ok(novaFormacao);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "DataInicio_Futura")
            {
                return BadRequest(new { code = "DataInicio_Futura", message = "A data de início não pode ser futura." });
            }
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro Interno ao adicionar Formacao: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a adição." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListaFormacaos()
    {
        var Formacaos = await _FormacaoService.ListarFormacoes();
        return Ok(Formacaos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarFormacaoPorId(Guid id)
    {
        var Formacao = await _FormacaoService.ListarFormacaoPorID(id);
        if (Formacao == null)
        {
            return NotFound(new { message = "Formação não encontrada" });
        }
        return Ok(Formacao);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarFormacao([FromBody] Formacao formacao)
    {
        try
        {
            var atualizado = await _FormacaoService.AtualizarFormacao(formacao);

            if (atualizado == null)
            {
                return NotFound(new { message = "Formação não encontrada" });
            }
            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "DataInicio_Futura")
            {
                return BadRequest(new { code = "DataInicio_Futura", message = "A data de início não pode ser futura." });
            }

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro Interno ao atualizar Formacao: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpPut("Deletar/{idFormacao}")]
    public async Task<IActionResult> ExcluirFormacao(Guid idFormacao)
    {
        var excluido = await _FormacaoService.ExcluirFormacao(idFormacao);
        if (excluido == null)
        {
            return NotFound(new { message = "Formação não encontrada" });
        }
        return Ok(excluido);
    }

    [HttpGet("FormacaoPorCurriculo/{curriculoId}")]
    public async Task<IActionResult> ListarFormacoesPorCurriculo(Guid curriculoId)
    {
        var formacoes = await _FormacaoService.ListarFormacaoPorCurriculoId(curriculoId);
        return Ok(formacoes);
    }
}
