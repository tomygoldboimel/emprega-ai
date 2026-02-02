using System.Security.Cryptography;
using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class CurriculoService : ICurriculoService
    {
        private readonly AplicacaoContext _context;

        public CurriculoService(AplicacaoContext context)
        {
            _context = context;
        }

        public async Task<Curriculo> AdicionaCurriculo(Curriculo curriculo)
        {
            curriculo.Id = Guid.NewGuid();
            curriculo.Excluido = false;

            if (curriculo.DataNascimento != default)
            {
                curriculo.DataNascimento = DateTime.SpecifyKind((DateTime)curriculo.DataNascimento, DateTimeKind.Utc);
            }
            if (curriculo.Experiencias != null)
            {
                foreach (var exp in curriculo.Experiencias)
                {
                    exp.DataInicio = DateTime.SpecifyKind((DateTime)exp.DataInicio, DateTimeKind.Utc);
                    if (exp.DataFim.HasValue)
                        exp.DataFim = DateTime.SpecifyKind(exp.DataFim.Value, DateTimeKind.Utc);
                }
            }
            if (curriculo.DataNascimento > DateTime.UtcNow)
            {
                throw new ArgumentException("DataNascimento_Invalida");
            }

            _context.Curriculos.Add(curriculo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var innerError = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"Erro detalhado no POST: {innerError}");
                throw;
            }
            return curriculo;
        }

        public async Task<List<Curriculo>> ListarCurriculos()
        {
            var lista = await _context.Curriculos
                .Where(x => x.Excluido != true)
                .ToListAsync();

            if (lista.Count == 0)
            {
                return new List<Curriculo>();
            }

            return lista;
        }

        public async Task<Curriculo> ListarCurriculoPorID(Guid id)
        {
            return await _context.Curriculos
                .FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Curriculo> ListarCurriculoPorUsuario(Guid usuarioId)
        {
            var curriculo = await _context.Curriculos
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.Excluido != true);
            return curriculo;
        }

        public async Task<Curriculo> AtualizarCurriculo(Curriculo curriculo)
        {
            var c = await ListarCurriculoPorID(curriculo.Id);

            if (c == null)
            {
                return null;
            }
            if (curriculo.DataNascimento > DateTime.Today)
            {
                throw new ArgumentException("DataNascimento_Invalida");
            }
            _context.Entry(c).CurrentValues.SetValues(curriculo);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<Curriculo> ExcluirCurriculo(Curriculo curriculo)
        {
            var c = await ListarCurriculoPorID(curriculo.Id);

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