import api from '../api';
import type { Formacao } from 'src/models/Formacao';

const ROUTE = '/Formacao';

class FormacaoService {
  async adicionarFormacao(Formacao: Omit<Formacao, 'id' | 'ativo' | 'excluido'>): Promise<Formacao> {
    const response = await api.post<Formacao>(ROUTE, Formacao);
    return response.data;
  }

  async listarFormacoes(): Promise<Formacao[]> {
    const response = await api.get<Formacao[]>(ROUTE);
    return response.data;
  }

  async listarFormacaoPorId(id: string): Promise<Formacao> {
    const response = await api.get<Formacao>(`${ROUTE}/${id}`);
    return response.data;
  }
  async listarFormacoesPorCurriculoId(curriculoId: string): Promise<Formacao> {
    const response = await api.get<Formacao>(`${ROUTE}/FormacaoPorCurriculo/${curriculoId}`);
    return response.data;
  }
  async atualizarFormacao(Formacao: Formacao): Promise<Formacao> {
    const response = await api.put<Formacao>(`${ROUTE}/Atualizar`, Formacao);
    return response.data;
  }

  async excluirFormacao(idFormacao: String): Promise<Formacao> {
    const response = await api.put<Formacao>(`${ROUTE}/Deletar/${idFormacao}`);
    return response.data;
  }
}

export default new FormacaoService();