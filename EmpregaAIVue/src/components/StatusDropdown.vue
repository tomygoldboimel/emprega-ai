<template>
  <div class="status-dropdown" v-click-outside="fechar">
    <div class="dropdown-trigger" @click="aberto = !aberto">
      <span v-if="modelValue" class="valor-selecionado">{{ modelValue }}</span>
      <span v-else class="placeholder">Selecione</span>
      <svg 
        class="icone-seta" 
        :class="{ rotacionado: aberto }"
        width="16" 
        height="16" 
        viewBox="0 0 24 24" 
        fill="none" 
        stroke="currentColor" 
        stroke-width="2"
      >
        <polyline points="6 9 12 15 18 9"></polyline>
      </svg>
    </div>
    
    <div v-if="aberto" class="dropdown-lista">
      <div 
        v-for="status in statusOptions" 
        :key="status"
        class="dropdown-item"
        :class="{ selecionado: modelValue === status }"
        @click="selecionar(status)"
      >
        {{ status }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'StatusDropdown',
  props: {
    modelValue: {
      type: String,
      default: ''
    }
  },
  emits: ['update:modelValue'],
  data() {
    return {
      aberto: false,
      statusOptions: [
        'Cursando',
        'Concluído',
        'Trancado'
      ]
    }
  },
  directives: {
    'click-outside': {
      mounted(el, binding) {
        el.clickOutsideEvent = function(event) {
          if (!(el === event.target || el.contains(event.target))) {
            binding.value()
          }
        }
        document.addEventListener('click', el.clickOutsideEvent)
      },
      unmounted(el) {
        document.removeEventListener('click', el.clickOutsideEvent)
      }
    }
  },
  methods: {
    selecionar(status) {
      this.$emit('update:modelValue', status)
      this.aberto = false
    },
    fechar() {
      this.aberto = false
    }
  }
}
</script>

<style scoped>
.status-dropdown {
  position: relative;
  width: 100%;
}

.dropdown-trigger {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px 12px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.2s ease;
  height: 38px;
  font-size: 14px;
  box-sizing: border-box;
}

.dropdown-trigger:hover {
  border-color: #9ca3af;
}

.valor-selecionado {
  color: #111827;
  font-size: 14px;
  font-weight: 500;
  line-height: 1;
}

.placeholder {
  color: #9ca3af;
  font-size: 14px;
  line-height: 1;
}

.icone-seta {
  color: #6b7280;
  transition: transform 0.3s ease;
  flex-shrink: 0;
}

.icone-seta.rotacionado {
  transform: rotate(180deg);
}

.dropdown-lista {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  animation: aparecer 0.2s ease-out;
}

@keyframes aparecer {
  from {
    opacity: 0;
    transform: translateY(-8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.dropdown-item {
  padding: 10px 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  border-bottom: 1px solid #f3f4f6;
  font-size: 14px;
  color: #111827;
}

.dropdown-item:last-child {
  border-bottom: none;
}

.dropdown-item:hover {
  background: #f9fafb;
}

.dropdown-item.selecionado {
  background: #3b82f6;
  color: white;
  font-weight: 600;
}
</style>