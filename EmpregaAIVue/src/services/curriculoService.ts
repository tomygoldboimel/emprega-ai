import api from '../api';
import type { Curriculo } from 'src/models/Curriculo';

const ROUTE = '/Curriculo';

class CurriculoService {
  async adicionarCurriculo(Curriculo: Omit<Curriculo, 'id' | 'ativo' | 'excluido'>): Promise<Curriculo> {
    const response = await api.post<Curriculo>(ROUTE, Curriculo);
    return response.data;
  }

  async listarCurriculos(): Promise<Curriculo[]> {
    const response = await api.get<Curriculo[]>(ROUTE);
    return response.data;
  }

  async listarCurriculosPorUsuario(usuarioId: string): Promise<Curriculo[]> {
    const response = await api.get<Curriculo[]>(`${ROUTE}/usuario/${usuarioId}`);
    return response.data;
  }

  async listarCurriculoPorId(id: string): Promise<Curriculo> {
    const response = await api.get<Curriculo>(`${ROUTE}/${id}`);
    return response.data;
  }

  async atualizarCurriculo(Curriculo: Curriculo): Promise<Curriculo> {
    const response = await api.put<Curriculo>(`${ROUTE}/Atualizar`, Curriculo);
    return response.data;
  }

  async excluirCurriculo(Curriculo: Curriculo): Promise<Curriculo> {
    const response = await api.put<Curriculo>(`${ROUTE}/Deletar`, Curriculo);
    return response.data;
  }
}

export default new CurriculoService();