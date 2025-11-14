using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class FormacaoService : IFormacaoService
    {
        private readonly AplicacaoContext _context;

        public FormacaoService(AplicacaoContext context)
        {
            _context = context;
        }

        public async Task<Formacao> AdicionaFormacao(Formacao Formacao)
        {
            Formacao.Id = Guid.NewGuid();
            Formacao.Excluido = false;
            if (Formacao.DataInicio > DateTime.Today)
            {
                throw new ArgumentException("DataInicio_Futura");
            }
            _context.Formacoes.Add(Formacao);
            await _context.SaveChangesAsync();
            return Formacao;
        }

        public async Task<List<Formacao>> ListarFormacoes()
        {
            var lista = await _context.Formacoes
                .Where(x => x.Excluido != true)
                .ToListAsync();

            if (lista.Count == 0)
            {
                return new List<Formacao>();
            }

            return lista;
        }

        public async Task<Formacao> ListarFormacaoPorID(Guid id)
        {
            return await _context.Formacoes
                .FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Formacao> AtualizarFormacao(Formacao Formacao)
        {
            var c = await ListarFormacaoPorID(Formacao.Id);

            if (c == null)
            {
                return null;
            }
            if (Formacao.DataInicio > DateTime.Today)
            {
                throw new ArgumentException("DataInicio_Futura");
            }
            _context.Entry(c).CurrentValues.SetValues(Formacao);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<List<Formacao>> ListarFormacaoPorCurriculoId(Guid curriculoId)
        {
            var formacoes = await _context.Formacoes
                .Where(x => x.CurriculoId == curriculoId && x.Excluido != true)
                .OrderByDescending(x => x.DataInicio)
                .ToListAsync();

            return formacoes;
        }

        public async Task<Formacao> ExcluirFormacao(Guid idFormacao)
        {
            var c = await ListarFormacaoPorID(idFormacao);

            if (c == null)
            {
                return null;
            }

            c.Excluido = true;
            await _context.SaveChangesAsync();
            return c;
        }
    }
}