// src/services/FormacaoService.ts
import axios from 'axios';
import type { Formacao } from 'src/models/Formacao';

const API_URL = 'https://localhost:44388/api/Formacao';

class FormacaoService {
  async adicionarFormacao(Formacao: Omit<Formacao, 'id' | 'ativo' | 'excluido'>): Promise<Formacao> {
    const response = await axios.post<Formacao>(API_URL, Formacao);
    return response.data;
  }

  async listarFormacaos(): Promise<Formacao[]> {
    const response = await axios.get<Formacao[]>(API_URL);
    return response.data;
  }

  async listarFormacaoPorId(id: string): Promise<Formacao> {
    const response = await axios.get<Formacao>(`${API_URL}/${id}`);
    return response.data;
  }
  async listarFormacoesPorCurriculoId(curriculoId: string): Promise<Formacao> {
    const response = await axios.get<Formacao>(`${API_URL}/FormacaoPorCurriculo/${curriculoId}`);
    return response.data;
  }
  async atualizarFormacao(Formacao: Formacao): Promise<Formacao> {
    const response = await axios.put<Formacao>(`${API_URL}/Atualizar`, Formacao);
    return response.data;
  }

  async excluirFormacao(idFormacao: String): Promise<Formacao> {
    const response = await axios.put<Formacao>(`${API_URL}/Deletar/${idFormacao}`);
    return response.data;
  }
}

export default new FormacaoService();