import { ref, onUnmounted } from 'vue';

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
  const transcript = ref('');
  const interimTranscript = ref('');
  const error = ref<string | null>(null);
  const isSupported = ref(false);
  
  let recognition: any = null;

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

    recognition.lang = 'pt-BR';
    recognition.continuous = false;
    recognition.interimResults = true;
    recognition.maxAlternatives = 1;

    recognition.onresult = (event: SpeechRecognitionEvent) => {
      let interim = '';
      let final = '';

      for (let i = event.resultIndex; i < event.results.length; i++) {
        const result = event.results[i];
        const transcriptText = result[0].transcript;

        if (result.isFinal) {
          final += transcriptText;
        } else {
          interim += transcriptText;
        }
      }

      if (final) {
        transcript.value = final;
      }
      interimTranscript.value = interim;
    };

    recognition.onstart = () => {
      isListening.value = true;
      error.value = null;
      transcript.value = '';
      interimTranscript.value = '';
    };

    recognition.onend = () => {
      isListening.value = false;
    };

    recognition.onerror = (event: SpeechRecognitionErrorEvent) => {
      isListening.value = false;
      
      const errorMessages: Record<string, string> = {
        'no-speech': 'Não detectei nenhuma fala. Tente novamente.',
        'audio-capture': 'Microfone não encontrado. Verifique se está conectado.',
        'not-allowed': 'Permissão de microfone negada. Permita nas configurações do navegador.',
        'network': 'Erro de rede. Verifique sua conexão com a internet.',
        'aborted': 'Gravação cancelada.',
        'language-not-supported': 'Idioma não suportado.',
      };

      error.value = errorMessages[event.error] || `Erro desconhecido: ${event.error}`;
    };
  };

  const startRecording = async () => {
    if (!isSupported.value) {
      error.value = 'Reconhecimento de voz não suportado neste navegador';
      return;
    }

    try {
      await navigator.mediaDevices.getUserMedia({ audio: true });
      
      recognition.start();
    } catch (err: any) {
      
      if (err.name === 'NotAllowedError') {
        error.value = 'Você negou permissão para o microfone. Permita nas configurações do navegador.';
      } else if (err.name === 'NotFoundError') {
        error.value = 'Microfone não encontrado. Conecte um microfone e tente novamente.';
      } else {
        error.value = 'Erro ao acessar microfone.';
      }
    }
  };

  const stopRecording = () => {
    if (recognition && isListening.value) {
      recognition.stop();
    }
  };

  const cancelRecording = () => {
    if (recognition && isListening.value) {
      recognition.abort();
      transcript.value = '';
      interimTranscript.value = '';
    }
  };

  onUnmounted(() => {
    if (recognition && isListening.value) {
      recognition.abort();
    }
  });

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