using EmpregaAI.Models;
using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienciaController : ControllerBase
{
    private readonly IExperienciaService _ExperienciaService;

    public ExperienciaController(IExperienciaService ExperienciaService)
    {
        _ExperienciaService = ExperienciaService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionaExperiencia([FromBody] Experiencia experiencia)
    {
        try
        {
            var novaExperiencia = await _ExperienciaService.AdicionaExperiencia(experiencia);

            if (novaExperiencia == null)
            {
                return BadRequest(new { message = "Não foi possível adicionar a experiência." });
            }

            return Ok(novaExperiencia);
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
            Console.Error.WriteLine($"Erro Interno ao adicionar Experiência: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a adição." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListaExperiencias()
    {
        var Experiencias = await _ExperienciaService.ListarExperiencias();
        return Ok(Experiencias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarExperienciaPorId(Guid id)
    {
        var Experiencia = await _ExperienciaService.ListarExperienciaPorID(id);
        if (Experiencia == null)
        {
            return NotFound(new { message = "Experiência não encontrada" });
        }
        return Ok(Experiencia);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarExperiencia([FromBody] Experiencia Experiencia)
    {
        try
        {
            var atualizado = await _ExperienciaService.AtualizarExperiencia(Experiencia);

            if (atualizado == null)
            {
                return NotFound(new { message = "Experiência não encontrada" });
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
            Console.Error.WriteLine($"Erro Interno ao atualizar Experiência: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpPut("Deletar/{idExperiencia}")]
    public async Task<IActionResult> ExcluirExperiencia(Guid idExperiencia)
    {
        var excluido = await _ExperienciaService.ExcluirExperiencia(idExperiencia);
        if (excluido == null)
        {
            return NotFound(new { message = "Experiência não encontrada" });
        }
        return Ok(excluido);
    }

    [HttpGet("ExperienciaPorCurriculo/{curriculoId}")]
    public async Task<IActionResult> ListarExperienciasPorCurriculo(Guid curriculoId)
    {
        var experiencias = await _ExperienciaService.ListarExperienciasPorCurriculoId(curriculoId);
        return Ok(experiencias);
    }
}