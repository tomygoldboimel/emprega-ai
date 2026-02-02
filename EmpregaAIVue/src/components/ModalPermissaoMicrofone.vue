<template>
  <div v-if="isOpen" class="modal-overlay" @click="fechar">
    <div class="modal-container" @click.stop>
      <div class="modal-content">
        <div class="audio-icon-container">
           <img src="@/assets/icons/microphoneIcon.svg" alt="Microfone" class="microphone-icon"/>
        </div>

        <h3 class="modal-title" @click="$emit('falar', 'Permitir Microfone')">
          Permitir Microfone
        </h3>
        
        <p class="modal-message" @click="$emit('falar', message)">{{message}}
        </p>
        
        <div class="modal-buttons">
          <button class="btn-voltar" @click="fechar">
            <img src="@/assets/icons/returnIcon.svg" alt="Return"/>
          </button>
          <button class="btn-confirmar" @click="confirmar">
            <img src="@/assets/icons/checkIcon.svg" alt="Check" class="check-icon"/>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    isOpen: { type: Boolean, required: true },
    message: {
      type: String,
      default: 'Seu navegador não possui acesso ao microfone. Configure suas permissões para acessar esse recurso.'
    },
  },
  emits: ['permitido', 'fechar', 'falar', 'erro'],
  methods: {
    fechar() {
      this.$emit('fechar');
    },
    async confirmar() {
      try {
        const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        
        stream.getTracks().forEach(track => track.stop());
        
        this.$emit('permitido');
        this.fechar();
      } catch (err) {
        console.error("Erro ao pedir permissão:", err);
        this.$emit('erro', 'Permissão negada ou microfone não encontrado.');
        this.fechar();
      }
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed; top: 0; left: 0; right: 0; bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex; justify-content: center; align-items: center;
  z-index: 10000;
}

.modal-container {
  background: white; border-radius: 24px;
  width: 90%; max-width: 350px;
  height: 350px;
  animation: slideUp 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.modal-content {
  padding: 2.5rem 1.5rem;
  display: flex; flex-direction: column; align-items: center; text-align: center;
}

.microphone-icon{
    height: 50px;
}

.check-icon{
    filter: brightness(0) invert(1);
}

.audio-icon-container {
  position: relative; margin-bottom: 1rem;
}

.mic-main-icon {
  width: 60px; height: 60px;
  background: #000;
}

.pulse-ring {
  position: absolute; top: 0; left: 0; width: 60px; height: 60px;
  border: 4px solid #000; border-radius: 50%;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% { transform: scale(1); opacity: 1; }
  100% { transform: scale(1.6); opacity: 0; }
}

.modal-title { font-size: 1.5rem; font-weight: 800; margin-bottom: 0.5rem; color: #000;}
.modal-message { font-size: 1rem; color: #555; line-height: 1.5; margin-bottom: 2rem; }

.modal-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  margin-bottom: 25px;
  width: 100%;
}

.btn-confirmar {
  background: #000; color: #fff; border: none;
  padding: 1rem; border-radius: 12px; font-weight: 700; width: 100%;
}

.btn-voltar,
.btn-confirmar {
  width: auto;
  min-width: 140px;
  height: 55px;
  padding: 0.75rem 2rem;
  border-radius: 12px;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  border: 2px solid #333;
}
</style>