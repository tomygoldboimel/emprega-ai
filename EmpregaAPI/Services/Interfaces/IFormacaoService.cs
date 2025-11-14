using EmpregaAI.Models;
using EmpregaAPI.Models;

namespace EmpregaAI.Services.Interfaces
{
    public interface IFormacaoService
    {
        Task<Formacao> AdicionaFormacao(Formacao Formacao);
        Task<List<Formacao>> ListarFormacoes();
        Task<Formacao> ListarFormacaoPorID(Guid id);
        Task<Formacao> AtualizarFormacao(Formacao Formacao);
        Task<Formacao> ExcluirFormacao(Guid idFormacao);
        Task<List<Formacao>> ListarFormacaoPorCurriculoId(Guid curriculoId);
    }
}
