<template>
  <div class="auth-page">
    
    <div class="auth-container">
      <div class="form-wrapper">
        <div class="form-content">
          <BackButton />
          
          <h1>Digite o código</h1>
          <p class="subtitle">
            Enviamos um código para  <br>
            <strong>{{ maskedPhone }}</strong>.
          </p>

          <div class="otp-container">
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
            <span>Não recebeu o código?</span>
            <a 
              @click="handleResend" 
              :class="{ 'disabled': timer > 0 }"
            >
              {{ timer > 0 ? `Aguarde ${timer}s` : 'Reenviar' }}
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
import { verificarCodigo } from '@/services/codigoService'

export default {
  name: 'VerificationCode',
  components: {
    BackButton
  },
  data() {
    return {
      otp: ['', '', '', '', '', ''], // Array para os 6 dígitos
      loading: false,
      error: '',
      success: '',
      timer: 0,
      phone: localStorage.getItem('telefoneVerificacao') || '5511999998888' // Fallback
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
    // Gerencia a digitação e avança o foco
    handleInput(index, event) {
      const value = event.target.value;
      
      // Garante que é número
      if (!/^\d*$/.test(value)) {
        this.otp[index] = '';
        return;
      }

      // Avança o foco se digitou algo
      if (value && index < 5) {
        this.$refs.otpInput[index + 1].focus();
      }
    },

    // Gerencia o Backspace para voltar o foco
    handleBackspace(index, event) {
      if (!this.otp[index] && index > 0) {
        this.otp[index - 1] = '';
        this.$refs.otpInput[index - 1].focus();
      }
    },

    // Permite colar o código inteiro (Ctrl+V)
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

      try {
        const usuario = await verificarCodigo(this.phone, codigoCompleto);

        // 🔐 salva usuário logado
        localStorage.setItem('usuarioLogado', JSON.stringify(usuario));

        this.success = 'Código verificado com sucesso!';

        setTimeout(() => {
          localStorage.removeItem('telefoneVerificacao');
          this.$router.push('/curriculo');
        }, 800);

      } catch (e) {
        this.error = 'Código inválido. Tente novamente.';
        this.otp = ['', '', '', '', '', ''];
        this.$refs.otpInput[0].focus();
      } finally {
        this.loading = false;
      }
    },


    handleResend() {
      if (this.timer > 0) return;
      
      // Lógica de reenvio aqui
      this.timer = 30;
      this.success = 'Novo código enviado!';
      setTimeout(() => this.success = '', 3000);

      const interval = setInterval(() => {
        this.timer--;
        if (this.timer === 0) clearInterval(interval);
      }, 1000);
    }
  },
  mounted() {
    // Foca no primeiro input ao carregar
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
  background: #fafafa; /* Fundo cinza claro conforme Login_new.vue */
}

.auth-container {
  position: relative;
  z-index: 1;
  width: 100%;
  max-width: 440px; /* Ajustado para corresponder ao Login_new.vue */
  display: flex;
  flex-direction: column;
  align-items: center;
}

.form-wrapper {
  width: 100%;
  padding: 0 40px;
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

.otp-container input.filled {
  border-color: #374151;
  background-color: #fafafa;
}

/* Botão Estilo "Cadastro" (Preto) */
.btn-submit {
  width: 100%;
  padding: 16px;
  border-radius: 12px; /* Pouco menos arredondado que os inputs */
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
  cursor: default;
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

.btn-back {
  position: absolute;
  top: 20px;
  left: 20px;
  z-index: 10;
}
</style>