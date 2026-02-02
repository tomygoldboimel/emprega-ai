import { ref, computed, onUnmounted, watch } from 'vue';

export function useSpeechSynthesis() {
  const isSupported = ref(false);
  const isPlaying = ref(false);
  const isPaused = ref(false);
  const status = ref<'init' | 'playing' | 'pause' | 'end'>('init');
  
  const text = ref('');
  const pitch = ref(1);
  const rate = ref(1);
  const volume = ref(1);
  const voice = ref<SpeechSynthesisVoice | null>(null);
  const voices = ref<SpeechSynthesisVoice[]>([]);
  
  const error = ref<string | null>(null);
  
  const boundaryStart = ref(0);
  const boundaryEnd = ref(0);
  
  let synth: SpeechSynthesis;
  let utterance: SpeechSynthesisUtterance | null = null;

  const textSegments = computed(() => {
    const fullText = text.value || '';
    const startIndex = Math.max(0, Math.min(boundaryStart.value, fullText.length));
    const endIndex = Math.max(startIndex, Math.min(boundaryEnd.value, fullText.length));
    
    return {
      leadingText: fullText.slice(0, startIndex),
      highlightedText: fullText.slice(startIndex, endIndex),
      trailingText: fullText.slice(endIndex),
    };
  });

  const checkSupport = () => {
    if ('speechSynthesis' in window) {
      isSupported.value = true;
      synth = window.speechSynthesis;
      loadVoices();
      
      if (speechSynthesis.onvoiceschanged !== undefined) {
        speechSynthesis.onvoiceschanged = loadVoices;
      }
    } else {
      isSupported.value = false;
      error.value = 'Seu navegador não suporta síntese de voz (Text-to-Speech).';
    }
  };

  const loadVoices = () => {
    voices.value = synth.getVoices();
    
    const portugueseVoice = voices.value.find(v => 
      v.lang.startsWith('pt-BR') || v.lang.startsWith('pt')
    );
    
    if (portugueseVoice) {
      voice.value = portugueseVoice;
    } else if (voices.value.length > 0) {
      voice.value = voices.value[0];
    }
  };

  const onBoundary = (event: SpeechSynthesisEvent) => {
    const { charIndex, charLength } = event;
    const startIndex = charIndex;
    let endIndex = charIndex;
    
    if (typeof charLength === 'number' && charLength > 0) {
      endIndex = startIndex + charLength;
    } else {
      const fullText = text.value || '';
      const remainingText = fullText.slice(startIndex);
      const firstWordMatch = remainingText.match(/^\S+/);
      endIndex = startIndex + (firstWordMatch ? firstWordMatch[0].length : 0);
    }
    
    boundaryStart.value = startIndex;
    boundaryEnd.value = endIndex;
  };

  const resetBoundary = () => {
    boundaryStart.value = 0;
    boundaryEnd.value = 0;
  };

  const createUtterance = () => {
    if (!text.value) {
      error.value = 'Nenhum texto para falar';
      return null;
    }

    utterance = new SpeechSynthesisUtterance(text.value);
    
    utterance.pitch = pitch.value;
    utterance.rate = rate.value;
    utterance.volume = volume.value;
    if (voice.value) {
      utterance.voice = voice.value;
    }

    // Eventos
    utterance.onstart = () => {
      isPlaying.value = true;
      isPaused.value = false;
      status.value = 'playing';
      error.value = null;
    };

    utterance.onend = () => {
      isPlaying.value = false;
      isPaused.value = false;
      status.value = 'end';
      resetBoundary();
    };

    utterance.onerror = (event) => {
      isPlaying.value = false;
      isPaused.value = false;
      status.value = 'end';
      error.value = `Erro na síntese de voz: ${event.error}`;
    };

    utterance.onpause = () => {
      isPaused.value = true;
      status.value = 'pause';
    };

    utterance.onresume = () => {
      isPaused.value = false;
      status.value = 'playing';
    };

    utterance.onboundary = onBoundary;

    return utterance;
  };

  const speak = () => {
    if (!isSupported.value) {
      error.value = 'Síntese de voz não suportada';
      return;
    }

    if (isPlaying.value) {
      synth.cancel();
    }

    resetBoundary();
    
    const newUtterance = createUtterance();
    if (newUtterance) {
      synth.speak(newUtterance);
    }
  };

  const play = () => {
    if (isPaused.value) {
      resume();
    } else {
      speak();
    }
  };

  const pause = () => {
    if (isPlaying.value && !isPaused.value) {
      synth.pause();
    }
  };

  const resume = () => {
    if (isPaused.value) {
      synth.resume();
    }
  };

  const stop = () => {
    if (isPlaying.value || isPaused.value) {
      synth.cancel();
      isPlaying.value = false;
      isPaused.value = false;
      status.value = 'end';
      resetBoundary();
    }
  };

  const getPortugueseVoices = computed(() => {
    return voices.value.filter(v => 
      v.lang.startsWith('pt-BR') || v.lang.startsWith('pt')
    );
  });

  watch(() => status.value, (newStatus) => {
    if (newStatus === 'end') {
      resetBoundary();
    }
  });

  onUnmounted(() => {
    if (isPlaying.value) {
      synth.cancel();
    }
  });

  checkSupport();

  return {
    isSupported,
    isPlaying,
    isPaused,
    status,
    error,
    
    text,
    pitch,
    rate,
    volume,
    voice,
    voices,
    
    boundaryStart,
    boundaryEnd,
    textSegments,
    
    speak,
    play,
    pause,
    resume,
    stop,
    
    getPortugueseVoices,
  };
}