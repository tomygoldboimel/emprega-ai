using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class ExperienciaService : IExperienciaService
    {
        private readonly AplicacaoContext _context;

        public ExperienciaService(AplicacaoContext context)
        {
            _context = context;
        }

        public async Task<Experiencia> AdicionaExperiencia(Experiencia Experiencia)
        {
            Experiencia.Id = Guid.NewGuid();
            Experiencia.Excluido = false;
            if (Experiencia.DataInicio > DateTime.Today)
            {
                throw new ArgumentException("DataInicio_Futura");
            }
            _context.Experiencias.Add(Experiencia);
            await _context.SaveChangesAsync();
            return Experiencia;
        }

        public async Task<List<Experiencia>> ListarExperiencias()
        {
            var lista = await _context.Experiencias
                .Where(x => x.Excluido != true)
                .ToListAsync();

            if (lista.Count == 0)
            {
                return new List<Experiencia>();
            }

            return lista;
        }

        public async Task<Experiencia> ListarExperienciaPorID(Guid id)
        {
            return await _context.Experiencias
                .FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Experiencia> AtualizarExperiencia(Experiencia Experiencia)
        {
            var c = await ListarExperienciaPorID(Experiencia.Id);

            if (c == null)
            {
                return null;
            }
            if (Experiencia.DataInicio > DateTime.Today)
            {
                throw new ArgumentException("DataInicio_Futura");
            }

            _context.Entry(c).CurrentValues.SetValues(Experiencia);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<List<Experiencia>> ListarExperienciasPorCurriculoId(Guid curriculoId)
        {
            var experiencias = await _context.Experiencias
                .Where(x => x.CurriculoId == curriculoId && x.Excluido != true)
                .OrderByDescending(x => x.DataInicio)
                .ToListAsync();

            return experiencias;
        }

        public async Task<Experiencia> ExcluirExperiencia(Guid idExperiencia)
        {
            var c = await ListarExperienciaPorID(idExperiencia);

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