// src/services/CurriculoService.ts
import axios from 'axios';
import type { Curriculo } from 'src/models/Curriculo';

const API_URL = 'https://localhost:44388/api/Curriculo';

class CurriculoService {
  async adicionarCurriculo(Curriculo: Omit<Curriculo, 'id' | 'ativo' | 'excluido'>): Promise<Curriculo> {
    const response = await axios.post<Curriculo>(API_URL, Curriculo);
    return response.data;
  }

  async listarCurriculos(): Promise<Curriculo[]> {
    const response = await axios.get<Curriculo[]>(API_URL);
    return response.data;
  }

  async listarCurriculosPorUsuario(usuarioId: string): Promise<Curriculo[]> {
    const response = await axios.get<Curriculo[]>(`${API_URL}/usuario/${usuarioId}`);
    return response.data;
  }

  async listarCurriculoPorId(id: string): Promise<Curriculo> {
    const response = await axios.get<Curriculo>(`${API_URL}/${id}`);
    return response.data;
  }

  async atualizarCurriculo(Curriculo: Curriculo): Promise<Curriculo> {
    const response = await axios.put<Curriculo>(`${API_URL}/Atualizar`, Curriculo);
    return response.data;
  }

  async excluirCurriculo(Curriculo: Curriculo): Promise<Curriculo> {
    const response = await axios.put<Curriculo>(`${API_URL}/Deletar`, Curriculo);
    return response.data;
  }
}

export default new CurriculoService();