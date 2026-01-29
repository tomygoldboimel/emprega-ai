<template>
  <button
    @click="falar"
    :class="['btn-audio', tamanho, { 'falando': falando, 'carregando': carregando }]"
    :disabled="carregando"
    type="button"
  >
    <svg 
      v-if="!falando && !carregando"
      xmlns="http://www.w3.org/2000/svg" 
      width="20" 
      height="20" 
      viewBox="0 0 24 24" 
      fill="none" 
      stroke="currentColor" 
      stroke-width="2"
    >
      <polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"></polygon>
      <path d="M15.54 8.46a5 5 0 0 1 0 7.07"></path>
    </svg>
    
    <div v-if="carregando" class="spinner"></div>
    
    <svg 
      v-if="falando && !carregando"
      xmlns="http://www.w3.org/2000/svg" 
      width="18" 
      height="18" 
      viewBox="0 0 24 24" 
      fill="currentColor"
    >
      <rect x="6" y="6" width="12" height="12" rx="2"></rect>
    </svg>
  </button>
</template>

<script>
import axios from 'axios';

export default {
  name: 'BotaoAudio',
  props: {
    texto: { 
      type: String, 
      required: true 
    },
    tamanho: { 
      type: String, 
      default: 'normal' 
    }
  },
  data() {
    return {
      falando: false,
      carregando: false,
      audio: null
    }
  },
  methods: {
    async falar() {
      if (this.falando) {
        this.parar();
        return;
      }

      if (!this.texto || this.texto.trim() === '') {
        return;
      }

      try {
        this.carregando = true;

        const response = await axios.get('/api/audio/sintetizar', {
          params: { texto: this.texto },
          responseType: 'blob'
        });

        const audioBlob = new Blob([response.data], { type: 'audio/mpeg' });
        const audioUrl = URL.createObjectURL(audioBlob);

        this.audio = new Audio(audioUrl);

        this.audio.onplay = () => {
          this.carregando = false;
          this.falando = true;
        };

        this.audio.onended = () => {
          this.falando = false;
          URL.revokeObjectURL(audioUrl);
        };

        this.audio.onerror = (e) => {
          this.carregando = false;
          this.falando = false;
          alert('Erro ao reproduzir áudio');
        };

        await this.audio.play();

      } catch (error) {
        this.carregando = false;
        this.falando = false;
        alert('Erro ao carregar áudio do servidor');
      }
    },

    parar() {
      if (this.audio) {
        this.audio.pause();
        this.audio.currentTime = 0;
      }
      this.falando = false;
      this.carregando = false;
    }
  },

  beforeUnmount() {
    this.parar();
  }
}
</script>

<style scoped>
.btn-audio {
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  color: #ffffff;
  transition: all 0.2s ease;
  min-width: 40px;
  min-height: 40px;
}

.btn-audio:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-audio:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.15);
}

.btn-audio.falando {
  animation: pulse-audio 1.5s ease-in-out infinite;
}

.spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes pulse-audio {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.6; }
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>