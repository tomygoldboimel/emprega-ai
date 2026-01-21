// src/composables/useSpeechRecognition.ts
import { ref, onUnmounted } from 'vue';

// Interface para tipagem
interface SpeechRecognitionEvent extends Event {
  results: SpeechRecognitionResultList;
  resultIndex: number;
}

interface SpeechRecognitionErrorEvent extends Event {
  error: string;
  message: string;
}

export function useSpeechRecognition() {
  const isListening = ref(false);
  const transcript = ref(''); // Texto final
  const interimTranscript = ref(''); // Texto parcial (enquanto fala)
  const error = ref<string | null>(null);
  const isSupported = ref(false);
  
  let recognition: any = null;

  // Verificar se o navegador suporta
  const checkSupport = () => {
    const SpeechRecognition = 
      (window as any).SpeechRecognition || 
      (window as any).webkitSpeechRecognition;
    
    if (SpeechRecognition) {
      isSupported.value = true;
      recognition = new SpeechRecognition();
      setupRecognition();
    } else {
      isSupported.value = false;
      error.value = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
    }
  };

  const setupRecognition = () => {
    if (!recognition) return;

    // Configurações
    recognition.lang = 'pt-BR'; // Português do Brasil
    recognition.continuous = false; // Para quando o usuário parar de falar
    recognition.interimResults = true; // Mostra resultado enquanto fala
    recognition.maxAlternatives = 1;

    // Quando tem resultado
    recognition.onresult = (event: SpeechRecognitionEvent) => {
      let interim = '';
      let final = '';

      // Processar resultados
      for (let i = event.resultIndex; i < event.results.length; i++) {
        const result = event.results[i];
        const transcriptText = result[0].transcript;

        if (result.isFinal) {
          final += transcriptText;
        } else {
          interim += transcriptText;
        }
      }

      // Atualizar refs
      if (final) {
        transcript.value = final;
      }
      interimTranscript.value = interim;
    };

    // Quando começa a gravar
    recognition.onstart = () => {
      isListening.value = true;
      error.value = null;
      transcript.value = '';
      interimTranscript.value = '';
      console.log('🎤 Gravação iniciada');
    };

    // Quando termina
    recognition.onend = () => {
      isListening.value = false;
      console.log('🛑 Gravação finalizada');
    };

    // Quando dá erro
    recognition.onerror = (event: SpeechRecognitionErrorEvent) => {
      isListening.value = false;
      
      // Mapear erros para mensagens amigáveis
      const errorMessages: Record<string, string> = {
        'no-speech': 'Não detectei nenhuma fala. Tente novamente.',
        'audio-capture': 'Microfone não encontrado. Verifique se está conectado.',
        'not-allowed': 'Permissão de microfone negada. Permita nas configurações do navegador.',
        'network': 'Erro de rede. Verifique sua conexão com a internet.',
        'aborted': 'Gravação cancelada.',
        'language-not-supported': 'Idioma não suportado.',
      };

      error.value = errorMessages[event.error] || `Erro desconhecido: ${event.error}`;
      console.error('❌ Erro de reconhecimento:', event.error);
    };
  };

  // Iniciar gravação
  const startRecording = async () => {
    if (!isSupported.value) {
      error.value = 'Reconhecimento de voz não suportado neste navegador';
      return;
    }

    try {
      // Solicitar permissão do microfone
      await navigator.mediaDevices.getUserMedia({ audio: true });
      
      // Iniciar reconhecimento
      recognition.start();
    } catch (err: any) {
      console.error('Erro ao iniciar gravação:', err);
      
      if (err.name === 'NotAllowedError') {
        error.value = 'Você negou permissão para o microfone. Permita nas configurações do navegador.';
      } else if (err.name === 'NotFoundError') {
        error.value = 'Microfone não encontrado. Conecte um microfone e tente novamente.';
      } else {
        error.value = 'Erro ao acessar microfone.';
      }
    }
  };

  // Parar gravação
  const stopRecording = () => {
    if (recognition && isListening.value) {
      recognition.stop();
    }
  };

  // Cancelar gravação
  const cancelRecording = () => {
    if (recognition && isListening.value) {
      recognition.abort();
      transcript.value = '';
      interimTranscript.value = '';
    }
  };

  // Limpar ao desmontar componente
  onUnmounted(() => {
    if (recognition && isListening.value) {
      recognition.abort();
    }
  });

  // Inicializar
  checkSupport();

  return {
    isSupported,
    isListening,
    transcript,
    interimTranscript,
    error,
    startRecording,
    stopRecording,
    cancelRecording,
  };
}