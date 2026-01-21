<template>
  <div class="cidade-dropdown" v-click-outside="fechar">
    <div 
      class="dropdown-trigger" 
      @click="toggleDropdown"
      :class="{ desabilitado: !cidades || cidades.length === 0 }"
    >
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
      <div class="filtro-container">
        <input 
          type="text"
          v-model="filtro"
          placeholder="Buscar cidade..."
          class="filtro-input"
          @click.stop
        />
      </div>
      
      <div v-if="cidadesFiltradas && cidadesFiltradas.length > 0">
        <div 
          v-for="cidade in cidadesFiltradas" 
          :key="cidade.id"
          class="dropdown-item"
          :class="{ selecionado: modelValue === cidade.nome }"
          @click="selecionar(cidade.nome)"
        >
          {{ cidade.nome }}
        </div>
      </div>
      
      <div v-else class="dropdown-item desabilitado">
        <span v-if="!cidades || cidades.length === 0">Nenhuma cidade disponível</span>
        <span v-else>Nenhuma cidade encontrada</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CidadeDropdown',
  props: {
    modelValue: {
      type: String,
      default: ''
    },
    cidades: {
      type: Array,
      default: () => []
    }
  },
  emits: ['update:modelValue'],
  data() {
    return {
      aberto: false,
      filtro: ''
    }
  },
  computed: {
    cidadesFiltradas() {
      if (!this.cidades) return [];
      return this.cidades.filter(c => 
        c.nome.toLowerCase().includes(this.filtro.toLowerCase())
      );
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
    toggleDropdown() {
      if (!this.cidades || this.cidades.length === 0) {
        this.aberto = false
        return
      }
      this.aberto = !this.aberto
    },
    selecionar(nome) {
      this.$emit('update:modelValue', nome)
      this.aberto = false
      this.filtro = ''
    },
    fechar() {
      this.aberto = false
      this.filtro = ''
    }
  }
}
</script>

<style scoped>
.cidade-dropdown {
  position: relative;
  width: 100%;
}

.dropdown-trigger {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px 12px;
  padding-right: 37px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.2s ease;
  height: 38px;
  font-size: 14px;
  box-sizing: border-box;
}

.dropdown-trigger:hover:not(.desabilitado) {
  border-color: #9ca3af;
}

.dropdown-trigger.desabilitado {
  opacity: 0.6;
  cursor: default;
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
  max-height: 250px;
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

.filtro-container {
  padding: 8px;
  border-bottom: 1px solid #f3f4f6;
  position: sticky;
  top: 0;
  background: white;
  border-radius: 8px 8px 0 0;
}

.filtro-input {
  width: 100%;
  padding: 8px 10px;
  border: 1px solid #e5e7eb;
  border-radius: 6px;
  font-size: 13px;
  box-sizing: border-box;
  transition: border-color 0.2s ease;
}

.filtro-input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.1);
}

.filtro-input::placeholder {
  color: #d1d5db;
}

.dropdown-item {
  padding: 10px 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  border-bottom: 1px solid #f3f4f6;
  color: #111827;
  font-size: 14px;
}

.dropdown-item:last-child {
  border-bottom: none;
}

.dropdown-item:hover:not(.desabilitado) {
  background: #f9fafb;
}

.dropdown-item.selecionado {
  background: #f3f4f6;
  font-weight: 600;
}

.dropdown-item.desabilitado {
  background: #fafbfc;
  color: #9ca3af;
  cursor: not-allowed;
  padding: 12px;
  text-align: center;
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
