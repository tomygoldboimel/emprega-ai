// src/services/iaFormattingService.ts
import axios, { AxiosError } from 'axios';

interface GroqMessage {
  role: 'system' | 'user' | 'assistant';
  content: string;
}

interface GroqRequest {
  model: string;
  messages: GroqMessage[];
  temperature: number;
  max_tokens: number;
}

interface GroqResponse {
  choices: Array<{
    message: {
      content: string;
    };
  }>;
}

class IAFormattingService {
  private groqApiKey: string = import.meta.env.VITE_GROQ_API_KEY;
  private groqApiUrl: string = 'https://api.groq.com/openai/v1/chat/completions';

  /**
   * Formata a descrição de atividades profissionais usando IA
   */
  async formatarDescricaoProfissional(descricao: string, cargo?: string): Promise<string> {
    try {
      const prompt = this.criarPrompt(descricao, cargo);
      
      const requestData: GroqRequest = {
        model: 'llama-3.3-70b-versatile',
        messages: [
          {
            role: 'system',
            content: 'Você é um especialista em RH e redação de currículos profissionais. Sua função é transformar descrições de atividades em textos profissionais, objetivos e impactantes para currículos.'
          },
          {
            role: 'user',
            content: prompt
          }
        ],
        temperature: 0.7,
        max_tokens: 500
      };

      const response = await axios.post<GroqResponse>(
        this.groqApiUrl,
        requestData,
        {
          headers: {
            'Authorization': `Bearer ${this.groqApiKey}`,
            'Content-Type': 'application/json'
          }
        }
      );

      return response.data.choices[0].message.content.trim();
      
    } catch (error) {
      console.error('Erro ao formatar com IA:', error);
      
      if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<any>;
        if (axiosError.response) {
          console.error('Erro da API:', axiosError.response.data);
          throw new Error(`Erro da API: ${axiosError.response.data.error?.message || 'Erro desconhecido'}`);
        } else if (axiosError.request) {
          throw new Error('Erro de conexão com a API. Verifique sua internet.');
        }
      }
      
      throw new Error('Erro ao configurar a requisição.');
    }
  }

  /**
   * Cria o prompt otimizado para formatação profissional
   */
  private criarPrompt(descricao: string, cargo?: string): string {
  let contexto = '';
  
  if (cargo) {
    contexto = `para o cargo/função de ${cargo}`;
  } else {
    contexto = 'de trabalho ou atividade profissional';
  }
  
  return `Transforme a seguinte descrição ${contexto} em um texto profissional e objetivo para currículo.

DESCRIÇÃO ORIGINAL:
${descricao}

INSTRUÇÕES:
- Use substantivos de ação no início de cada frase (execução, desenvolvimento, atuação, contribuição, etc.)
- Seja objetivo e conciso
- Destaque responsabilidades e atividades realizadas
- Valorize QUALQUER tipo de trabalho (formal, informal, autônomo, temporário)
- Use bullet points (•) se houver múltiplas atividades
- Mantenha tom profissional e respeitoso
- Máximo 3-4 linhas ou bullets
- NÃO invente informações que não estão no texto original
- Use português brasileiro formal
- Mesmo para trabalhos informais, use linguagem profissional
- Responda APENAS com o texto formatado, sem explicações adicionais

TEXTO FORMATADO:`;
}
}

export default new IAFormattingService();