using EmpregaAI.Models.Resume;

namespace EmpregaAI.Services.Interfaces
{
    public interface IProcessadorGroqService
    {
        Task<CurriculoUpload> ProcessResumeTextAsync(string resumeText);
    }
}
