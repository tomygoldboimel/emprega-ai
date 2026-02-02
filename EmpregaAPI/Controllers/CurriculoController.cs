using EmpregaAI.Models;
using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurriculoController : ControllerBase
{
    private readonly ICurriculoService _curriculoService;

    public CurriculoController(ICurriculoService curriculoService)
    {
        _curriculoService = curriculoService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionaCurriculo([FromBody] Curriculo curriculo)
    {
        try
        {
            var criado = await _curriculoService.AdicionaCurriculo(curriculo);
            return Ok(criado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListaCurriculos()
    {
        var curriculos = await _curriculoService.ListarCurriculos();
        return Ok(curriculos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarCurriculoPorId(Guid id)
    {
        var curriculo = await _curriculoService.ListarCurriculoPorID(id);
        if (curriculo == null)
        {
            return NotFound(new { message = "Currículo não encontrado" });
        }
        return Ok(curriculo);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarCurriculo([FromBody] Curriculo curriculo)
    {
        try
        {
            var atualizado = await _curriculoService.AtualizarCurriculo(curriculo);

            if (atualizado == null)
            {
                return NotFound(new { message = "Currículo não encontrado" });
            }
            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro Interno ao atualizar Curriculo: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpPut("Deletar")]
    public async Task<IActionResult> ExcluirCurriculo([FromBody] Curriculo curriculo)
    {
        var excluido = await _curriculoService.ExcluirCurriculo(curriculo);
        if (excluido == null)
        {
            return NotFound(new { message = "Currículo não encontrado" });
        }
        return Ok(excluido);
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ListarCurriculosPorUsuario(Guid usuarioId)
    {
        var curriculos = await _curriculoService.ListarCurriculoPorUsuario(usuarioId);
        return Ok(curriculos);
    }
}