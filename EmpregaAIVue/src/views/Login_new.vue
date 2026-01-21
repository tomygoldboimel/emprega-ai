<template>
  <div class="auth-page">
    <div class="auth-container">
      <div class="form-wrapper">
        <div class="form-content">
          <div class="icon-box">
            <img src="/profileIcon.svg" alt="User Icon" />
          </div>
          
          <h1>Bem-vindo</h1>
          <p class="subtitle">Insira seu telefone para começar</p>

          <div class="form-group">
            <label>Telefone</label>
            <input 
              type="tel" 
              v-model="cadastroTelefone"
              @input="formatarTelefone"
              @keyup.enter="handleCadastro"
              placeholder="(XX) XXXXX-XXXX"
            />
          </div>

          <button 
            class="btn-submit" 
            @click="handleCadastro"
            :disabled="loading"
          >
            <span v-if="!loading">Confirmar</span>
            <div v-else class="spinner"></div>
          </button>

          <div v-if="cadastroError" class="alert-error">
            {{ cadastroError }}
          </div>

          <div v-if="cadastroSuccess" class="alert-success">
            {{ cadastroSuccess }}
          </div>
        </div>
      </div>
    </div>

    <div v-if="errorMessage" class="alert alert-error">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
import { enviarCodigo } from '@/services/codigoService';
import usuarioService from '@/services/usuarioService';
import Login from './Login.vue';

export default {
  name: 'Cadastro',
  data() {
    return {
      loading: false,
      cadastroTelefone: '',
      cadastroError: '',
      cadastroSuccess: '',
      errorMessage: ''
    }
  },
  methods: {
    formatarTelefone(event) {
      let valor = event.target.value.replace(/\D/g, '');

      if (valor.length > 11) {
        valor = valor.slice(0, 11);
      }

      if (valor.length <= 10) {
        valor = valor.replace(
          /(\d{2})(\d{4})(\d{0,4})/,
          '($1) $2-$3'
        );
      } else {
        valor = valor.replace(
          /(\d{2})(\d{5})(\d{0,4})/,
          '($1) $2-$3'
        );
      }

      this.cadastroTelefone = valor;
    },
    async handleCadastro() {
      this.cadastroError = '';
      this.cadastroSuccess = '';

      const telefoneRegex = /^\d{10,11}$/;
      if (!telefoneRegex.test(this.cadastroTelefone.replace(/\D/g, ''))) {
        this.cadastroError = "Por favor, insira um telefone válido.";
        return;
      }

      this.loading = true;
      try {
        const user = await usuarioService.login(this.cadastroTelefone.replace(/\D/g, ''));

        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
        }

        localStorage.setItem('telefoneVerificacao', this.cadastroTelefone.replace(/\D/g, ''));
        this.$router.push('/curriculo');
      } catch (error) {
        console.log('Erro no login:', error);
        this.errorMessage = 'Erro ao fazer login. Tente novamente.';
      } finally {
        this.loading = false;
      }
      // try {
      //   await enviarCodigo(this.cadastroTelefone.replace(/\D/g, ''));
        
      //   this.cadastroSuccess = 'Código enviado com sucesso!';
      //   localStorage.setItem('telefoneVerificacao', this.cadastroTelefone.replace(/\D/g, ''));

      //   setTimeout(() => {
      //     this.limparFormularioCadastro();
      //     this.$router.push('/verificar-codigo');
      //   }, 1500);

      // } catch (error) {
      //   if (error.response?.status === 400) {
      //     this.errorMessage = 'Telefone já cadastrado. Redirecionando ao Login';
      //     setTimeout(() => {
      //       this.errorMessage = '';
      //       this.limparFormularioCadastro();
      //       this.$router.push('/login');
      //     }, 2000);
      //   } else if (error.response) {
      //     this.cadastroError = error.response.data.message || 'Erro ao enviar código';
      //   } else if (error.request) {
      //     this.cadastroError = 'Erro de conexão. Verifique se a API está rodando.';
      //   } else {
      //     this.cadastroError = 'Erro inesperado ao enviar código';
      //   }
      // } finally {
      //   this.loading = false;
      // }
    },

    limparFormularioCadastro() {
      this.cadastroTelefone = '';
    }
  }
}
</script>

<style scoped>
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
  padding: 0 40px;
}

.form-content {
  width: 100%;
  text-align: center;
  background: white;
  padding: 48px;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.icon-box {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;
  background: #000;
  color: #fff;
}

.icon-box img {
  width: 32px;
  height: 32px;
}

.form-content h1 {
  font-size: 36px;
  font-weight: 600;
  margin-bottom: 8px;
  letter-spacing: -0.02em;
  color: #111827;
}

.subtitle {
  font-size: 16px;
  opacity: 0.7;
  margin-bottom: 24px;
  color: #6b7280;
}

.form-group {
  margin-bottom: 20px;
  text-align: left;
}

.form-group label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 8px;
  color: #374151;
}

.form-group input {
  width: 100%;
  padding: 14px 16px;
  border-radius: 10px;
  font-size: 15px;
  border: 1.5px solid #e5e7eb;
  font-family: inherit;
  transition: all 0.2s;
  background: #fff;
  color: #111827;
}

.form-group input:focus {
  outline: none;
  border-color: #000;
}

.btn-submit {
  width: 80%;
  padding: 14px;
  border-radius: 10px;
  font-size: 16px;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
  margin-top: 8px;
  margin-left: auto;
  margin-right: auto;
  min-height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #000;
  color: #fff;
}

.btn-submit:hover:not(:disabled) {
  background: #1f2937;
  transform: translateY(-1px);
}

.btn-submit:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

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

.alert-error,
.alert-success {
  padding: 12px 16px;
  border-radius: 10px;
  font-size: 14px;
  margin-top: 16px;
  animation: slideDown 0.3s ease;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
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

.footer-text {
  text-align: center;
  margin-top: 24px;
  font-size: 14px;
  color: #6b7280;
}

.footer-text a {
  font-weight: 600;
  cursor: pointer;
  margin-left: 4px;
  transition: opacity 0.2s;
  color: #000;
  text-decoration: none;
}

.footer-text a:hover {
  opacity: 0.7;
}

.alert {
  position: fixed;
  bottom: 24px;
  left: 50%;
  transform: translateX(-50%);
  padding: 16px 24px;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 500;
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
  z-index: 9999;
  animation: slideUp 0.3s ease;
  max-width: 90%;
  text-align: center;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateX(-50%) translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0);
  }
}

@media (max-width: 768px) {
  .form-wrapper {
    padding: 0 24px;
  }

  .form-content {
    padding: 32px 24px;
  }

  .form-content h1 {
    font-size: 28px;
  }
}
</style>