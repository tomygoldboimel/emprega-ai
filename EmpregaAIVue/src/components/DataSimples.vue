<template>
  <div class="data-simples">
    <label v-if="label" @click="falarElemento">{{ label }}</label>
    
    <!-- Botão que abre o modal -->
    <div class="data-display" @click="abrirModal">
      <span v-if="dataFormatada" class="data-texto">{{ dataFormatada }}</span>
      <span v-else class="data-placeholder">Selecionar data</span>
      <img src="@/assets/icons/calendarIcon.svg" alt="Calendário" class="icone" />
    </div>
    
    <BotaoMicrofone 
      v-if="permitirVoz"
      :isRecording="gravando" 
      @toggle="$emit('toggleGravacao')"
      class="btn-voz-data"
    />
    
    <!-- Modal de seleção -->
    <div v-if="modalAberto" class="modal-overlay" @click="fecharModal">
      <div class="modal-data" @click.stop>
        <div class="modal-header">
          <h3>{{ tituloModal }}</h3>
          <button @click="fecharModal" class="btn-fechar">×</button>
        </div>
        
        <div class="modal-corpo">
          <!-- Seletor de DIA -->
          <div class="secao-selector">
            <label class="label-grande">Dia</label>
            <div class="grid-numeros">
              <button 
                v-for="d in diasDoMes" 
                :key="d"
                :class="['btn-numero', { ativo: diaTemp === d }]"
                @click="selecionarDia(d)"
              >
                {{ d }}
              </button>
            </div>
          </div>
          
          <!-- Seletor de MÊS -->
          <div class="secao-selector">
            <label class="label-grande">Mês</label>
            <div class="grid-meses">
              <button 
                v-for="(mes, index) in meses" 
                :key="index"
                :class="['btn-mes', { ativo: mesTemp === index + 1 }]"
                @click="selecionarMes(index + 1)"
              >
                {{ mes }}
              </button>
            </div>
          </div>
          
          <!-- Seletor de ANO -->
          <div class="secao-selector">
            <label class="label-grande">Ano</label>
            <div class="scroll-anos">
              <button 
                v-for="ano in anosDisponiveis" 
                :key="ano"
                :class="['btn-ano', { ativo: anoTemp === ano }]"
                @click="selecionarAno(ano)"
              >
                {{ ano }}
              </button>
            </div>
          </div>
        </div>
        
        <div class="modal-footer">
          <div v-if="dataTemporaria" class="preview-data">
            {{ dataTemporariaFormatada }}
          </div>
          <button 
            @click="confirmarData" 
            class="btn-confirmar"
            :disabled="!dataCompleta"
          >
            Confirmar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BotaoMicrofone from '@/components/BotaoMicrofone.vue';

export default {
  name: 'DataSimples',
  components: { BotaoMicrofone },
  props: {
    modelValue: String,
    label: String,
    tituloModal: { type: String, default: 'Selecione a data' },
    anoInicio: { type: Number, default: 1940 },
    anoFim: { type: Number, default: new Date().getFullYear() },
    permitirVoz: { type: Boolean, default: true },
    gravando: { type: Boolean, default: false }
  },
  data() {
    return {
      modalAberto: false,
      diaTemp: null,
      mesTemp: null,
      anoTemp: null,
      meses: [
        'Janeiro', 'Fevereiro', 'Março', 'Abril', 
        'Maio', 'Junho', 'Julho', 'Agosto',
        'Setembro', 'Outubro', 'Novembro', 'Dezembro'
      ]
    }
  },
  computed: {
    dataFormatada() {
      if (!this.modelValue) return '';
      const [ano, mes, dia] = this.modelValue.split('-');
      return `${dia}/${mes}/${ano}`;
    },
    
    dataTemporaria() {
      if (this.diaTemp && this.mesTemp && this.anoTemp) {
        const dia = String(this.diaTemp).padStart(2, '0');
        const mes = String(this.mesTemp).padStart(2, '0');
        return `${this.anoTemp}-${mes}-${dia}`;
      }
      return null;
    },
    
    dataTemporariaFormatada() {
      if (!this.dataTemporaria) return '';
      return `${this.diaTemp} de ${this.meses[this.mesTemp - 1]} de ${this.anoTemp}`;
    },
    
    dataCompleta() {
      return this.diaTemp && this.mesTemp && this.anoTemp;
    },
    
    diasDoMes() {
      if (!this.mesTemp) return 31;
      const diasPorMes = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
      return diasPorMes[this.mesTemp - 1];
    },
    
    anosDisponiveis() {
      const anos = [];
      for (let i = this.anoFim; i >= this.anoInicio; i--) {
        anos.push(i);
      }
      return anos;
    }
  },
  watch: {
    modelValue: {
      immediate: true,
      handler(valor) {
        if (valor) {
          const [ano, mes, dia] = valor.split('-');
          this.anoTemp = parseInt(ano);
          this.mesTemp = parseInt(mes);
          this.diaTemp = parseInt(dia);
        }
      }
    }
  },
  methods: {
    abrirModal() {
      this.modalAberto = true;
      if (this.mostrarTutorial) {
        this.falarTexto(this.tituloModal);
      }
    },
    
    fecharModal() {
      this.modalAberto = false;
    },
    
    selecionarDia(dia) {
      this.diaTemp = dia;
      if (this.mostrarTutorial) {
        this.falarTexto(`Dia ${dia}`);
      }
    },
    
    selecionarMes(mes) {
      this.mesTemp = mes;
      // Ajustar dia se necessário
      if (this.diaTemp > this.diasDoMes) {
        this.diaTemp = this.diasDoMes;
      }
      if (this.mostrarTutorial) {
        this.falarTexto(this.meses[mes - 1]);
      }
    },
    
    selecionarAno(ano) {
      this.anoTemp = ano;
      if (this.mostrarTutorial) {
        this.falarTexto(`Ano ${ano}`);
      }
    },
    
    confirmarData() {
      if (this.dataCompleta) {
        this.$emit('update:modelValue', this.dataTemporaria);
        if (this.mostrarTutorial) {
          this.falarTexto(`Data confirmada: ${this.dataTemporariaFormatada}`);
        }
        this.fecharModal();
      }
    },
    
    falarTexto(texto) {
      this.$emit('falar', texto);
    },
    
    falarElemento(event) {
      const texto = event.target.innerText;
      this.$emit('falar', texto);
    }
  }
}
</script>

<style scoped>
.data-simples {
  position: relative;
  width: 100%;
}

.data-display {
  width: 100%;
  padding: 14px 50px 14px 14px;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 16px;
  background: white;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: space-between;
  transition: all 0.2s;
  min-height: 52px;
}

.data-display:active {
  border-color: #000;
  background: #fafafa;
}

.data-texto {
  color: #111827;
  font-weight: 500;
  font-size: 17px;
}

.data-placeholder {
  color: #9ca3af;
  font-size: 16px;
}

.icone {
  width: 20px;
  height: 20px;
  opacity: 0.4;
}

.btn-voz-data {
  position: absolute;
  right: 45px;
  top: 50%;
  transform: translateY(-50%);
}

/* MODAL */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  padding: 20px;
  animation: fadeIn 0.2s;
}

.modal-data {
  background: white;
  border-radius: 20px;
  width: 100%;
  max-width: 500px;
  max-height: 90vh;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  animation: slideUp 0.3s;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.modal-header {
  padding: 20px;
  border-bottom: 1px solid #e5e7eb;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h3 {
  font-size: 20px;
  font-weight: 600;
  color: #111827;
  margin: 0;
}

.btn-fechar {
  width: 36px;
  height: 36px;
  border: none;
  background: #f3f4f6;
  border-radius: 50%;
  font-size: 28px;
  line-height: 1;
  cursor: pointer;
  color: #6b7280;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-fechar:active {
  background: #e5e7eb;
}

.modal-corpo {
  flex: 1;
  overflow-y: auto;
  padding: 20px;
}

.secao-selector {
  margin-bottom: 24px;
}

.label-grande {
  font-size: 16px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 12px;
  display: block;
}

/* GRID DE DIAS */
.grid-numeros {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 8px;
}

.btn-numero {
  aspect-ratio: 1;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 12px;
  font-size: 18px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  transition: all 0.15s;
  min-height: 50px;
}

.btn-numero:active {
  transform: scale(0.95);
}

.btn-numero.ativo {
  background: #000;
  color: white;
  border-color: #000;
}

/* GRID DE MESES */
.grid-meses {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 10px;
}

.btn-mes {
  padding: 14px;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 12px;
  font-size: 15px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  transition: all 0.15s;
  min-height: 54px;
}

.btn-mes:active {
  transform: scale(0.97);
}

.btn-mes.ativo {
  background: #000;
  color: white;
  border-color: #000;
}

.scroll-anos {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 10px;
  max-height: 300px;
  overflow-y: auto;
  padding: 4px;
}

.btn-ano {
  padding: 14px;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  color: #374151;
  cursor: pointer;
  transition: all 0.15s;
  min-height: 52px;
}

.btn-ano:active {
  transform: scale(0.97);
}

.btn-ano.ativo {
  background: #000;
  color: white;
  border-color: #000;
}

.modal-footer {
  padding: 20px;
  border-top: 1px solid #e5e7eb;
}

.preview-data {
  text-align: center;
  font-size: 16px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 12px;
  padding: 12px;
  background: #f9fafb;
  border-radius: 8px;
}

.btn-confirmar {
  width: 100%;
  padding: 16px;
  background: #000;
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 17px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-confirmar:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.btn-confirmar:active:not(:disabled) {
  transform: scale(0.98);
}

/* MOBILE */
@media (max-width: 768px) {
  .modal-data {
    max-height: 85vh;
    border-radius: 20px 20px 0 0;
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    max-width: none;
  }
  
  .grid-numeros {
    gap: 6px;
  }
  
  .btn-numero {
    min-height: 46px;
    font-size: 16px;
  }
  
  .grid-meses {
    grid-template-columns: repeat(2, 1fr);
  }
  
  .scroll-anos {
    grid-template-columns: repeat(3, 1fr);
    max-height: 250px;
  }
}
</style>