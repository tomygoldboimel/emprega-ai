import axios from 'axios';

const API_URL = 'https://emprega-ai-production.up.railway.app/api/resume';

// ===== INTERFACES =====
export interface DadosPessoaisProcessados {
  nomeCompleto?: string;
  dataNascimento?: string;
  telefone?: string;
  email?: string;
  endereco?: string;
  cidade?: string;
  estado?: string;
  linkedIn?: string;
  gitHub?: string;
}

export interface ExperienciaProcessada {
  empresa?: string;
  cargo?: string;
  dataInicio?: string;
  dataFim?: string;
  empregoAtual?: boolean;
  descricao?: string;
}

export interface FormacaoProcessada {
  instituicao?: string;
  curso?: string;
  nivel?: string;
  status?: string;
  dataInicio?: string;
  dataConclusao?: string;
}

export interface CurriculoProcessado {
  dadosPessoais?: DadosPessoaisProcessados;
  experiencias?: ExperienciaProcessada[];
  formacoes?: FormacaoProcessada[];
}

// ===== SERVICE =====
class ResumeUploadService {
  async uploadResume(file: File): Promise<CurriculoProcessado> {
    const formData = new FormData();
    formData.append('file', file);

    const response = await axios.post<CurriculoProcessado>(
      `${API_URL}/upload`,
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }
    );

    return response.data;
  }
  validarEstruturaProcessada(data: any): data is CurriculoProcessado {
    // Verifica se data existe e é um objeto
    if (!data || typeof data !== 'object') return false;

    // Verifica se dadosPessoais (se existir) é um objeto
    if (data.dadosPessoais && typeof data.dadosPessoais !== 'object') return false;

    // Verifica se os arrays (se existirem) são realmente arrays
    if (data.experiencias && !Array.isArray(data.experiencias)) return false;
    if (data.formacoes && !Array.isArray(data.formacoes)) return false;
    if (data.certificacoes && !Array.isArray(data.certificacoes)) return false;

    return true; // ✅ Passou em todas as validações
  }
}


export default new ResumeUploadService();