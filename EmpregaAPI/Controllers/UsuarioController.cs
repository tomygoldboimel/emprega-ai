using EmpregaAI.Services;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _UsuarioService;

    public UsuarioController(IUsuarioService UsuarioService)
    {
        _UsuarioService = UsuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionaUsuario([FromBody] Usuario usuario)
    {
        var novoUsuario = await _UsuarioService.AdicionaUsuario(usuario);
        if (novoUsuario == null)
        {
            return BadRequest(new { message = "Email já cadastrado" });
        }
        return Ok(novoUsuario);
    }
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logout realizado com sucesso" });
    }

    [HttpGet("verificar-sessao")]
    public IActionResult VerificarSessao()
    {
        var usuarioId = HttpContext.Session.GetString("UsuarioId");

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Ok(new { autenticado = false });
        }

        return Ok(new { autenticado = true });
    }

    [HttpGet]
    public async Task<IActionResult> ListaUsuarios()
    {
        var Usuarios = await _UsuarioService.ListarUsuarios();
        return Ok(Usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarUsuarioPorId(Guid id)
    {
        var Usuario = await _UsuarioService.ListarUsuarioPorID(id);
        if (Usuario == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        return Ok(Usuario);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarUsuario([FromBody] Usuario Usuario)
    {
        var atualizado = await _UsuarioService.AtualizarUsuario(Usuario);
        if (atualizado == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        return Ok(atualizado);
    }

    [HttpPut("Deletar")]
    public async Task<IActionResult> ExcluirUsuario([FromBody] Usuario Usuario)
    {
        var excluido = await _UsuarioService.ExcluirUsuario(Usuario);
        if (excluido == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        return Ok(excluido);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            return BadRequest(new { message = "Email e senha são obrigatórios" });
        }

        var usuario = await _UsuarioService.Login(email, senha);

        if (usuario == null)
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }
        HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
        return Ok(usuario);
    }
}