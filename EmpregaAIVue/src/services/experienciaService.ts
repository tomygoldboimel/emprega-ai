import axios from 'axios';
import type { Experiencia } from 'src/models/Experiencia';

const API_URL = 'https://emprega-ai-production.up.railway.app/api/Experiencia';

class ExperienciaService {
  async adicionarExperiencia(Experiencia: Omit<Experiencia, 'id'>): Promise<Experiencia> {
    const response = await axios.post<Experiencia>(API_URL, Experiencia);
    return response.data;
  }

  async listarExperiencias(): Promise<Experiencia[]> {
    const response = await axios.get<Experiencia[]>(API_URL);
    return response.data;
  }

  async listarExperienciaPorId(id: string): Promise<Experiencia> {
    const response = await axios.get<Experiencia>(`${API_URL}/${id}`);
    return response.data;
  }

  async listarExperienciaPorIdCurriculo(curriculoId: string): Promise<Experiencia> {
    const response = await axios.get<Experiencia>(`${API_URL}/ExperienciaPorCurriculo/${curriculoId}`);
    return response.data;
  }

  async atualizarExperiencia(Experiencia: Experiencia): Promise<Experiencia> {
    const response = await axios.put<Experiencia>(`${API_URL}/Atualizar`, Experiencia);
    return response.data;
  }

  async excluirExperiencia(idExperiencia: string): Promise<Experiencia> {
    const response = await axios.put<Experiencia>(`${API_URL}/Deletar/${idExperiencia}`);
    return response.data;
  }
}

export default new ExperienciaService();