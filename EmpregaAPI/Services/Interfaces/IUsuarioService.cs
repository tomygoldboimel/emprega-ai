using EmpregaAPI.Models;

namespace EmpregaAI.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> AdicionaUsuario(Usuario Usuario);
        Task<List<Usuario>> ListarUsuarios();
        Task<Usuario> ListarUsuarioPorID(Guid id);
        Task<Usuario> AtualizarUsuario(Usuario Usuario);
        Task<Usuario> ExcluirUsuario(Usuario Usuario);
        Task<Usuario> Login(string telefone);
    }
}
