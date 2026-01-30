<template>
  <div v-if="isOpen" class="modal-overlay" @click="fechar">
    <div class="modal-container" @click.stop>
      <div class="modal-content">
        <h3 class="modal-title" @click="$emit('falar', title)">{{ title }}</h3>
        
        <p class="modal-message" @click="$emit('falar', message)">{{ message }}</p>
        
        <div class="modal-buttons">
          <button class="btn-voltar" @click="fechar">
            Voltar
          </button>
          <button class="btn-confirmar" @click="confirmar">
            Confirmar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
export default {
  props: {
    isOpen: {
      type: Boolean,
      required: true
    },
    message: {
      type: String,
      default: 'Tem certeza que deseja encerrar sua sessão?'
    },
    title: {
      type: String,
      default: 'Sair'
    }
  },
  emits: ['confirmar', 'fechar', 'falar'],
  methods: {
    confirmar() {
      this.$emit('confirmar');
    },
    fechar() {
      this.$emit('fechar');
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.modal-container {
  background: white;
  border-radius: 20px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  max-width: 500px;
  width: 90%;
  height: 250px;
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateY(-50px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.modal-content {
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.modal-title {
  margin: 0;
  margin: 10px 10px -10px;
  font-size: 1.75rem;
  font-weight: 700;
  color: #000;
}

.modal-message {
  margin: 0px 10px;
  color: #333;
  font-size: 1rem;
  line-height: 1.6;
}

.modal-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  margin-top: 25px;
  width: 100%;
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

@media (max-width: 768px) {
  .modal-buttons {
    display: flex;
    gap: 1rem;
    justify-content: center;
    margin-top: 10px;
  }

  .modal-title {
    margin: 10px 10px -10px -5px;
  }

  .modal-message {
    margin: 0px 10px -10px -5px;
  }
}


.btn-voltar {
  background-color: white;
  color: #333;
}

.btn-voltar:hover {
  background-color: #f5f5f5;
}

.btn-confirmar {
  background-color: #000;
  color: white;
  border-color: #000;
}

.btn-confirmar:hover {
  background-color: #333;
  border-color: #333;
}

.btn-voltar:focus,
.btn-confirmar:focus {
  outline: 2px solid #3b82f6;
  outline-offset: 2px;
}
</style>