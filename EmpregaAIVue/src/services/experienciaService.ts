import api from '../api';
import type { Experiencia } from 'src/models/Experiencia';

const ROUTE = '/Experiencia';

class ExperienciaService {
  async adicionarExperiencia(experiencia: Omit<Experiencia, 'id'>): Promise<Experiencia> {
    const response = await api.post<Experiencia>(ROUTE, experiencia);
    return response.data;
  }

  async listarExperiencias(): Promise<Experiencia[]> {
    const response = await api.get<Experiencia[]>(ROUTE);
    return response.data;
  }

  async listarExperienciaPorId(id: string): Promise<Experiencia> {
    const response = await api.get<Experiencia>(`${ROUTE}/${id}`);
    return response.data;
  }

  async listarExperienciaPorIdCurriculo(curriculoId: string): Promise<Experiencia> {
    const response = await api.get<Experiencia>(`${ROUTE}/ExperienciaPorCurriculo/${curriculoId}`);
    return response.data;
  }

  async atualizarExperiencia(Experiencia: Experiencia): Promise<Experiencia> {
    const response = await api.put<Experiencia>(`${ROUTE}/Atualizar`, Experiencia);
    return response.data;
  }

  async excluirExperiencia(idExperiencia: string): Promise<Experiencia> {
    const response = await api.put<Experiencia>(`${ROUTE}/Deletar/${idExperiencia}`);
    return response.data;
  }
}

export default new ExperienciaService();