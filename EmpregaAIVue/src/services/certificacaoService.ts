import api from '../api';
import type { Certificacao } from 'src/models/Certificacao';

const ROUTE = '/Certificacao';

class CertificacaoService {
  async adicionarCertificacao(Certificacao: Omit<Certificacao, 'id' | 'ativo' | 'excluido'>): Promise<Certificacao> {
    const response = await api.post<Certificacao>(ROUTE, Certificacao);
    return response.data;
  }

  async listarCertificacaos(): Promise<Certificacao[]> {
    const response = await api.get<Certificacao[]>(ROUTE);
    return response.data;
  }

  async listarCertificacaoPorId(id: string): Promise<Certificacao> {
    const response = await api.get<Certificacao>(`${ROUTE}/${id}`);
    return response.data;
  }
  async listarCertificacoesPorCurriculoId(curriculoId: string): Promise<Certificacao> {
      const response = await api.get<Certificacao>(`${ROUTE}/CertificacaoPorCurriculo/${curriculoId}`);
      return response.data;
    }
  async atualizarCertificacao(Certificacao: Certificacao): Promise<Certificacao> {
    const response = await api.put<Certificacao>(`${ROUTE}/Atualizar`, Certificacao);
    return response.data;
  }

  async excluirCertificacao(idCertificacao: String): Promise<Certificacao> {
    const response = await api.put<Certificacao>(`${ROUTE}/Deletar/${idCertificacao}`);
    return response.data;
  }
}

export default new CertificacaoService();