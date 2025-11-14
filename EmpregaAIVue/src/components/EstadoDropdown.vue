<template>
  <div class="estado-dropdown" v-click-outside="fechar">
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
        v-for="estado in estados" 
        :key="estado.sigla"
        class="dropdown-item"
        :class="{ selecionado: modelValue === estado.sigla }"
        @click="selecionar(estado.sigla)"
      >
        <span class="sigla">{{ estado.sigla }}</span>
        <span class="nome">{{ estado.nome }}</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'EstadoDropdown',
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
      estados: [
        { sigla: 'AC', nome: 'Acre' },
        { sigla: 'AL', nome: 'Alagoas' },
        { sigla: 'AP', nome: 'Amapá' },
        { sigla: 'AM', nome: 'Amazonas' },
        { sigla: 'BA', nome: 'Bahia' },
        { sigla: 'CE', nome: 'Ceará' },
        { sigla: 'DF', nome: 'Distrito Federal' },
        { sigla: 'ES', nome: 'Espírito Santo' },
        { sigla: 'GO', nome: 'Goiás' },
        { sigla: 'MA', nome: 'Maranhão' },
        { sigla: 'MT', nome: 'Mato Grosso' },
        { sigla: 'MS', nome: 'Mato Grosso do Sul' },
        { sigla: 'MG', nome: 'Minas Gerais' },
        { sigla: 'PA', nome: 'Pará' },
        { sigla: 'PB', nome: 'Paraíba' },
        { sigla: 'PR', nome: 'Paraná' },
        { sigla: 'PE', nome: 'Pernambuco' },
        { sigla: 'PI', nome: 'Piauí' },
        { sigla: 'RJ', nome: 'Rio de Janeiro' },
        { sigla: 'RN', nome: 'Rio Grande do Norte' },
        { sigla: 'RS', nome: 'Rio Grande do Sul' },
        { sigla: 'RO', nome: 'Rondônia' },
        { sigla: 'RR', nome: 'Roraima' },
        { sigla: 'SC', nome: 'Santa Catarina' },
        { sigla: 'SP', nome: 'São Paulo' },
        { sigla: 'SE', nome: 'Sergipe' },
        { sigla: 'TO', nome: 'Tocantins' }
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
    selecionar(sigla) {
      this.$emit('update:modelValue', sigla)
      this.aberto = false
    },
    fechar() {
      this.aberto = false
    }
  }
}
</script>

<style scoped>
.estado-dropdown {
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
  line-height: 1;  /* ← Adicione para alinhar verticalmente */
}

.placeholder {
  color: #9ca3af;
  font-size: 14px;
  line-height: 1;  /* ← Adicione para alinhar verticalmente */
}

.icone-seta {
  color: #6b7280;
  transition: transform 0.3s ease;
  flex-shrink: 0;  /* ← Impede que o ícone encolha */
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
  max-height: 280px;
  overflow-y: auto;
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
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  border-bottom: 1px solid #f3f4f6;
}

.dropdown-item:last-child {
  border-bottom: none;
}

.dropdown-item:hover {
  background: #f9fafb;
}

.dropdown-item.selecionado {
  background: #f3f4f6;
  font-weight: 600;
}

.sigla {
  font-weight: 600;
  color: #111827;
  font-size: 14px;
  min-width: 32px;
}

.nome {
  color: #6b7280;
  font-size: 13px;
}

.dropdown-lista::-webkit-scrollbar {
  width: 6px;
}

.dropdown-lista::-webkit-scrollbar-track {
  background: #f3f4f6;
  border-radius: 3px;
}

.dropdown-lista::-webkit-scrollbar-thumb {
  background: #d1d5db;
  border-radius: 3px;
}

.dropdown-lista::-webkit-scrollbar-thumb:hover {
  background: #9ca3af;
}
</style>