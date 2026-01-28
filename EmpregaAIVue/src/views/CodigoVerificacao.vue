<template>
  <div class="auth-page">
    <div class="auth-container">
      <div class="form-wrapper">
        <div class="top-bar">
          <BackButton @click="voltarTela()" /> 
          
          <div class="right-actions">
            <BotaoDescricao class="btn-descricao" @toggle="handleTutorialToggle" :active="mostrarTutorial"/>
          </div>
        </div>
        <div class="form-content">
          
          <h1 @click="falarElemento">Digite o código</h1>
          <p class="subtitle" @click="falarTexto('Enviamos um código para ' + falarTelefone(this.phone))">
            Enviamos um código para  <br>
            <strong>{{ maskedPhone }}</strong>.
          </p>

          <div class="otp-container">
            <input
              type="text"
              autocomplete="one-time-code"
              inputmode="numeric"
              class="otp-hidden-input"
              @input="handleAutoFill"
            />
            <input 
              v-for="(digit, index) in otp" 
              :key="index"
              type="text" 
              inputmode="numeric"
              maxlength="1"
              ref="otpInput"
              v-model="otp[index]"
              @input="handleInput(index, $event)"
              @keydown.delete="handleBackspace(index, $event)"
              @paste="handlePaste"
              @click="garantirVisibilidade"
              @focus="garantirVisibilidade"
              :class="{ 'filled': otp[index] !== '' }"
            />
          </div>

          <button 
            class="btn-submit" 
            @click="handleVerify"
            :disabled="loading || !isComplete"
          >
            <span v-if="!loading">Confirmar</span>
            <div v-else class="spinner"></div>
          </button>

          <div class="footer-text">
            <span @click="falarTexto('Não recebeu o código? Reenviar')">Não recebeu o código?</span>
            <a 
              @click="handleResend" 
              :class="{ 'disabled': timer > 0 }"
            >
              {{ timer > 0 ? `Reenviar em 00:${timer}` : 'Reenviar código' }}
            </a>
          </div>
          
          <transition name="fade">
            <div v-if="error" class="alert-error">
              {{ error }}
            </div>
          </transition>

           <transition name="fade">
            <div v-if="success" class="alert-success">
              {{ success }}
            </div>
          </transition>

        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BackButton from '@/components/BackButton.vue'
import BotaoDescricao from '@/components/BotaoDescricao.vue'
import { verificarCodigo } from '@/services/codigoService'

export default {
  name: 'VerificationCode',
  components: {
    BackButton,
    BotaoDescricao
  },
  data() {
    return {
      otp: ['', '', '', '', '', ''], // Array para os 6 dígitos
      loading: false,
      error: '',
      success: '',
      timer: 0,
      phone: localStorage.getItem('telefoneVerificacao') || '5511999998888',
      mostrarTutorial: localStorage.getItem('audioDescricaoAtiva') === 'true', // Fallback
    }
  },
  computed: {
    // Mascara o telefone para exibição
    maskedPhone() {
      if (!this.phone) return '';
      const ddd = this.phone.slice(0, 4);
      const last = this.phone.slice(-2);
      return `+55${ddd}****-**${last}`; // Ajuste conforme seu formato real
    },
    isComplete() {
      return this.otp.every(digit => digit !== '');
    }
  },
  methods: {
    voltarTela() {
      this.pararAudioTutorial();
      this.$router.back();
    },
    handleTutorialToggle(ativo) {
      this.mostrarTutorial = ativo;
      // Atualiza o storage caso o usuário mude de ideia nesta tela
      localStorage.setItem('audioDescricaoAtiva', ativo);
      
      if (ativo) {
        this.executarBoasVindasNativo();
      } else {
        this.pararAudioTutorial();
      }
    },
    handleAutoFill(event) {
      const value = event.target.value;
      if (value && value.length === 6) {
        this.otp = value.split('');
        this.handleVerify();
      }
    },
    garantirVisibilidade(event) {
      const container = event.target.parentNode;

      setTimeout(() => {
        if (container && container.scrollIntoView) {
          container.scrollIntoView({
            behavior: 'smooth',
            block: 'center'
          });
        }
      }, 300);
    },
    falarTelefone(numero) {
      console.log(this.phone)
      if (!this.mostrarTutorial || !window.speechSynthesis) return;
      const numeroEspacado = numero.replace(/\D/g, '').split('').join(' ');
      return numeroEspacado;
    },
    falarElemento(event) {
      const texto = event.target.innerText;
      this.falarTexto(texto);
    },
    falarTexto(texto) {
      // Só fala se o modo tutorial estiver ligado
      if (!this.mostrarTutorial) return;

      if (!window.speechSynthesis) return;

      // Cancela falas anteriores para não encavalar
      window.speechSynthesis.cancel();

      const utterance = new SpeechSynthesisUtterance(texto);
      utterance.lang = 'pt-BR';
      utterance.rate = 1.0;

      // Tenta usar a voz do Google se disponível
      const voices = window.speechSynthesis.getVoices();
      const googleVoice = voices.find(v => v.lang === 'pt-BR' && v.name.includes('Google'));
      if (googleVoice) utterance.voice = googleVoice;

      window.speechSynthesis.speak(utterance);
    },

    executarBoasVindasNativo() {
      if (!window.speechSynthesis) return;
      window.speechSynthesis.cancel();

      const texto = "Clique nos títulos para ouví-los";
      this.audioTutorial = new SpeechSynthesisUtterance(texto);
      this.audioTutorial.lang = 'pt-BR';
      
      // Ajuste fino para soar menos robótico
      this.audioTutorial.rate = 0.9;  // Um pouco mais lento costuma soar mais natural
      this.audioTutorial.pitch = 1.0; // Tom da voz

      const selecionarMelhorVoz = () => {
        const vozes = window.speechSynthesis.getVoices();
        
        // Procura especificamente pelas vozes neurais (Google ou Premium)
        const melhorVoz = vozes.find(v => 
          v.lang === 'pt-BR' && 
          (v.name.includes('Google') || v.name.includes('Neural') || v.name.includes('Natural'))
        );

        if (melhorVoz) {
          this.audioTutorial.voice = melhorVoz;
        }
        
        window.speechSynthesis.speak(this.audioTutorial);
      };

      if (window.speechSynthesis.getVoices().length === 0) {
        window.speechSynthesis.onvoiceschanged = selecionarMelhorVoz;
      } else {
        selecionarMelhorVoz();
      }
    },

    pararAudioTutorial() {
      // Para todas as falas em execução no navegador
      if (window.speechSynthesis) {
        window.speechSynthesis.cancel();
      }
    },
    handleInput(index, event) {
      const value = event.target.value;
      
      if (!/^\d*$/.test(value)) {
        this.otp[index] = '';
        return;
      }

      if (value && index < 5) {
        this.$refs.otpInput[index + 1].focus();
      }
    },

    handleBackspace(index, event) {
      if (!this.otp[index] && index > 0) {
        this.otp[index - 1] = '';
        this.$refs.otpInput[index - 1].focus();
      }
    },

    handlePaste(event) {
      event.preventDefault();
      const pastedData = event.clipboardData.getData('text').slice(0, 6);
      
      if (/^\d+$/.test(pastedData)) {
        const digits = pastedData.split('');
        digits.forEach((digit, i) => {
          if (i < 6) this.otp[i] = digit;
        });
        
        // Foca no último campo preenchido ou no final
        const focusIndex = Math.min(digits.length, 5);
        this.$nextTick(() => {
           this.$refs.otpInput[focusIndex].focus();
        });
      }
    },

    async handleVerify() {
      this.error = '';
      this.loading = true;
      const codigoCompleto = this.otp.join('');
      this.pararAudioTutorial();
      try {
        const usuario = await verificarCodigo(this.phone, codigoCompleto);

        localStorage.setItem('usuarioLogado', JSON.stringify(usuario));

        this.success = 'Código verificado com sucesso!';
        if (this.mostrarTutorial) {
          this.falarTexto('Código verificado com sucesso!');
        }
        setTimeout(() => {
          localStorage.removeItem('telefoneVerificacao');
          this.$router.push('/curriculo');
        }, 1500);

      } catch (e) {
        if (this.mostrarTutorial) {
          this.falarTexto('Código inválido. Tente novamente.');
        }
        this.error = 'Código inválido. Tente novamente.';
        this.otp = ['', '', '', '', '', ''];
        this.$refs.otpInput[0].focus();
      } finally {
        this.loading = false;
      }
    },


    startTimer() {
      this.timer = 30; // Define 30 segundos
      const interval = setInterval(() => {
        if (this.timer > 0) {
          this.timer--;
        } else {
          clearInterval(interval);
        }
      }, 1000);
    },

    handleResend() {
      if (this.timer > 0) return;
      this.success = 'Novo código enviado!';
      setTimeout(() => this.success = '', 3000);
      
      this.startTimer();
    }
  },
  mounted() {
    this.startTimer();
    this.$nextTick(() => {
      if (this.$refs.otpInput && this.$refs.otpInput[0]) {
        this.$refs.otpInput[0].focus();
      }
    });
  }
}
</script>

<style scoped>
/* Reset Básico */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.auth-page {
  width: 100vw;
  height: 100vh;
  position: relative;
  overflow: hidden;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', sans-serif;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fafafa;
}

.auth-container {
  position: relative;
  z-index: 1;
  width: 100%;
  max-width: 440px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.form-wrapper {
  width: 100%;
  max-width: 420px; /* Alinha a largura da barra com a largura do card branco */
  padding: 0; 
  display: flex;
  flex-direction: column;
  align-items: center;
}

.top-bar {
  display: flex;
  justify-content: space-between; /* Empurra BackButton para esquerda e right-actions para direita */
  align-items: center;
  width: 100%;
  padding: 0 4px; /* Pequeno respiro nas bordas */
  margin-bottom: 20px;
  position: relative; /* Garante que não herde 'absolute' de outras classes */
}

.right-actions {
  display: flex;
  align-items: center;
  justify-content: flex-end; /* Reforça o alinhamento à direita */
}

/* Ajuste opcional para dispositivos móveis */
@media (max-width: 480px) {
  .top-bar {
    padding: 0 5px;
  }
}

.btn-back, .btn-descricao, .btn-excel, .btn-pdf {
  padding: 10px 20px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
}

.btn-back, .btn-descricao {
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
}

.btn-back:hover {
  background: #f9fafb;
}

.form-content {
  width: 420px;
  height: 500px;

  display: flex;
  flex-direction: column;
  justify-content: center;

  text-align: center;
  background: white;
  padding: 32px 48px;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

  position: relative;
}


/* Tipografia igual à referência */
.form-content h1 {
  font-size: 36px;
  font-weight: 600;
  margin-bottom: 12px;
  letter-spacing: -0.02em;
  color: #111827; /* Texto escuro */
}

.subtitle {
  font-size: 16px;
  color: #374151;
  opacity: 0.8;
  margin-bottom: 40px;
}

/* Estilo dos Inputs de Código */
.otp-container {
  display: flex;
  justify-content: center;
  gap: 14px;
  margin-bottom: 40px;
}

.otp-container input {
  width: 48px;
  height: 52px;
  border-radius: 10px;
  border: 2px solid #000;
  font-size: 22px;
  font-weight: 600;

  /* Centraliza o número e o cursor horizontalmente */
  text-align: center; 

  /* Centraliza verticalmente usando Flexbox */
  display: flex;
  align-items: center;
  justify-content: center;

  /* Garante que o padding não "empurre" o texto para os lados */
  padding: 0; 
  box-sizing: border-box;
}

.otp-container input:focus {
  border-color: #000;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.otp-hidden-input {
  position: absolute;
  opacity: 0;
  pointer-events: none;
  width: 1px;
}

.otp-container input.filled {
  border-color: #374151;
  background-color: #fafafa;
}

.btn-submit {
  width: 100%;
  padding: 16px;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
  background: #000;
  color: #fff;
  min-height: 54px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-submit:hover:not(:disabled) {
  background: #1f2937;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.btn-submit:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Footer Link */
.footer-text {
  margin-top: 24px;
  font-size: 14px;
  color: #6b7280;
}

.footer-text a {
  font-weight: 600;
  color: #00a8e8; /* Azul ciano conforme a imagem enviada */
  cursor: pointer;
  margin-left: 4px;
  transition: opacity 0.2s;
  text-decoration: none;
}

.footer-text a:hover:not(.disabled) {
  text-decoration: underline;
}

.footer-text a.disabled {
  color: #9ca3af;
  cursor: not-allowed;
  pointer-events: none; /* Impede cliques enquanto o timer corre */
  text-decoration: none;
}

/* Spinner e Alerts (Reutilizados) */
.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid #fff;
  border-radius: 50%;
  border-top-color: transparent;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.alert-error, .alert-success {
  position: fixed;
  bottom: 24px;
  left: 50%;
  transform: translateX(-50%);
  padding: 16px 24px;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 500;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
  z-index: 9999;
}

.alert-error {
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
}

.alert-success {
  background: #f0fdf4;
  color: #166534;
  border: 1px solid #bbf7d0;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.fade-enter-from, .fade-leave-to {
  opacity: 0;
  transform: translate(-50%, 20px);
}

/* Responsividade */
@media (max-width: 480px) {
  .otp-container {
    gap: 8px;
  }
  
  .otp-container input {
    height: 50px;
    font-size: 20px;
    border-radius: 10px;
  }

  .form-wrapper {
    padding: 0 24px;
  }
  
  .form-content h1 {
    font-size: 28px;
  }
}

</style>