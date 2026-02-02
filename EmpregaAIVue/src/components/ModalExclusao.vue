<template>
  <transition name="modal">
    <div v-if="show" class="modal-overlay" @click.self="fechar">
      <div class="modal-container">
        <div class="modal-header">
          <div :class="['modal-icon', type]">
            <img src="@/assets/icons/circleXIcon.svg" alt="Return"/>
          </div>
          <h3 @click="$emit('falar', title)">{{ title }}</h3>
        </div>

        <div class="modal-body">
          <p @click="$emit('falar', message)">{{ message }}</p>
        </div>

        <div class="modal-buttons">
          <button class="btn-voltar" @click="fechar">
            <img src="@/assets/icons/returnIcon.svg" alt="Return"/>
          </button>
          <button class="btn-excluir" @click="confirmar">
            <img src="@/assets/icons/trashIcon.svg" alt="Check" class="trash-icon"/>
          </button>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: 'ModalExclusao',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    title: {
      type: String,
      default: 'Confirmar ação'
    },
    message: {
      type: String,
      default: 'Tem certeza que deseja continuar?'
    },
    type: {
      type: String,
      default: 'aviso',
      validator: (value) => ['aviso', 'perigo'].includes(value)
    },
    confirmText: {
      type: String,
      default: 'Confirmar'
    },
    cancelText: {
      type: String,
      default: 'Cancelar'
    }
  },
  methods: {
    confirmar() {
      this.$emit('confirmar');
      this.fechar();
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
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  padding: 20px;
}

.modal-container {
  background: white;
  border-radius: 16px;
  width: 100%;
  max-width: 440px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
  animation: slideUp 0.3s ease-out;
}

.modal-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  margin-bottom: 25px;
  width: 100%;
}

.btn-voltar,
.btn-excluir {
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

.btn-voltar {
  background-color: white;
  color: #333;
}

.btn-voltar:hover {
  background-color: #f5f5f5;
}

.btn-excluir {
  background-color: #fd0000;
  color: white;
  border-color: #fd0000;
}

.btn-excluir:hover {
  background-color: #333;
  border-color: #333;
}

.btn-voltar:focus,
.btn-excluir:focus {
  outline: 2px solid #3b82f6;
  outline-offset: 2px;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.modal-header {
  padding: 24px 24px 16px;
  text-align: center;
}

.modal-icon {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 16px;
}

.modal-icon.aviso {
  background: #fef3c7;
  color: #f59e0b;
}

.modal-icon.perigo {
  background: #fee2e2;
  color: #ffffff;
}

.modal-header h3 {
  font-size: 18px;
  font-weight: 600;
  color: #111827;
  margin: 0;
}

.modal-body {
  padding: 0 24px 24px;
  text-align: center;
}

.trash-icon{
  filter: brightness(0) invert(1);
}

.modal-body p {
  font-size: 14px;
  color: #6b7280;
  line-height: 1.6;
  margin: 0;
}

.modal-footer {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
  padding: 0 24px 24px;
}

.btn-cancel {
  padding: 10px 16px;
  background: white;
  color: #374151;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-cancel:hover {
  background: #f9fafb;
  border-color: #9ca3af;
}

.btn-confirm {
  padding: 10px 16px;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-confirm.aviso {
  background: #f59e0b;
}

.btn-confirm.aviso:hover {
  background: #d97706;
}

.btn-confirm.perigo {
  background: #ffffff;
}

.btn-confirm.perigo:hover {
  background: #ffffff;
}

.btn-confirm:active {
  transform: scale(0.98);
}

.modal-enter-active, .modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-from, .modal-leave-to {
  opacity: 0;
}

@media (max-width: 480px) {
  .modal-footer {
    grid-template-columns: 1fr;
  }
  
  .btn-cancel {
    order: 2;
  }
  
  .btn-confirm {
    order: 1;
  }
}
</style>