<template>
  <transition name="modal">
    <div v-if="show" class="modal-overlay" @click.self="fechar">
      <div class="modal-container">
        <button class="modal-close" @click="fechar" aria-label="Fechar">
          ×
        </button>

        <div class="modal-header">
          <div :class="['modal-icon', type]">
            <svg
              v-if="type === 'aviso'"
              xmlns="http://www.w3.org/2000/svg"
              width="28"
              height="28"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/>
              <line x1="12" y1="9" x2="12" y2="13"/>
              <line x1="12" y1="17" x2="12.01" y2="17"/>
            </svg>
          </div>

          <h3>{{ title }}</h3>
        </div>

        <div class="modal-body">
          <p>{{ message }}</p>
        </div>

      </div>
    </div>
  </transition>
</template>


<script>
export default {
  name: 'AvisoDescricao',
  props: {
    show: {
      type: Boolean,
      default: false
    },
    title: {
      type: String,
      default: 'Cuidado'
    },
    message: {
      type: String,
      default: 'Ao importar o currículo, a formatação da descrição pode não ser mantida. Recomandamos revisar e ajustar os textos após a importação.'
    },
    type: {
      type: String,
      default: 'perigo',
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
  position: relative;
  background: white;
  border-radius: 12px; 
  width: 100%;
  max-width: 400px;
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
  width: 72px;
  height: 72px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 12px;
}

.modal-icon.aviso {
  background-color: #FEF3C7;
  color: #F59E0B;
}


.modal-icon.perigo {
  background: #fee2e2;
  color: #ef4444;
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

.modal-close {
  position: absolute;
  top: 12px;
  right: 12px;

  background: transparent;
  border: none;
  outline: none;

  width: 28px;
  height: 28px;

  font-size: 20px;
  font-weight: 600;
  line-height: 1;

  cursor: pointer;

  display: flex;
  align-items: center;
  justify-content: center;

  color: #000000;
}

.modal-close:hover {
  color: #111827;
}

.modal-close:focus {
  outline: none;
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
  background: #ef4444;
}

.btn-confirm.perigo:hover {
  background: #dc2626;
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