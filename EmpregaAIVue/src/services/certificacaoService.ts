import axios from 'axios';
import type { Certificacao } from 'src/models/Certificacao';

const API_URL = 'https://localhost:7274/api/Certificacao';

class CertificacaoService {
  async adicionarCertificacao(Certificacao: Omit<Certificacao, 'id' | 'ativo' | 'excluido'>): Promise<Certificacao> {
    const response = await axios.post<Certificacao>(API_URL, Certificacao);
    return response.data;
  }

  async listarCertificacaos(): Promise<Certificacao[]> {
    const response = await axios.get<Certificacao[]>(API_URL);
    return response.data;
  }

  async listarCertificacaoPorId(id: string): Promise<Certificacao> {
    const response = await axios.get<Certificacao>(`${API_URL}/${id}`);
    return response.data;
  }
  async listarCertificacoesPorCurriculoId(curriculoId: string): Promise<Certificacao> {
      const response = await axios.get<Certificacao>(`${API_URL}/CertificacaoPorCurriculo/${curriculoId}`);
      return response.data;
    }
  async atualizarCertificacao(Certificacao: Certificacao): Promise<Certificacao> {
    const response = await axios.put<Certificacao>(`${API_URL}/Atualizar`, Certificacao);
    return response.data;
  }

  async excluirCertificacao(idCertificacao: String): Promise<Certificacao> {
    const response = await axios.put<Certificacao>(`${API_URL}/Deletar/${idCertificacao}`);
    return response.data;
  }
}

export default new CertificacaoService();