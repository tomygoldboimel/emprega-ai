<template>
  <div class="auth-page">
    <div class="auth-container">
      <div class="form-wrapper">
        <div class="form-content">
          <div class="icon-box">
            <img src="/usericon.svg" alt="User Icon" />
          </div>
          
          <h1>Bem-vindo</h1>
          <p class="subtitle">Entre com sua conta</p>

          <div class="form-group">
            <label>Telefone</label>
            <input 
              type="tel" 
              v-model="loginTelefone"
              @keyup.enter="handleLogin"
            />
          </div>

          <button 
            class="btn-submit" 
            @click="handleLogin"
            :disabled="loading"
          >
            <span v-if="!loading">Entrar</span>
            <div v-else class="spinner"></div>
          </button>

          <div v-if="loginError" class="alert-error">
            {{ loginError }}
          </div>

          <div class="footer-text">
            <span>Não tem uma conta?</span>
            <a @click="$router.push('/cadastro')">Cadastre-se</a>
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
export default {
  name: 'Login',
  data() {
    return {
      loading: false,
      loginTelefone: '',
      loginError: '',
      errorMessage: ''
    }
  },
  methods: {
    async handleLogin() {
      this.loginError = '';
      
      if (!this.loginTelefone) {
        this.loginError = 'Preencha o telefone';
        return;
      }

      const telefoneRegex = /^\d{10,11}$/;
      if (!telefoneRegex.test(this.loginTelefone.replace(/\D/g, ''))) {
        this.loginError = 'Telefone inválido (10 ou 11 dígitos)';
        return;
      }

      this.loading = true;
      
      try {
        localStorage.setItem('telefoneVerificacao', this.loginTelefone.replace(/\D/g, ''));
        this.$router.push('/verificar-codigo');
      } catch (error) {
        this.loginError = 'Erro ao processar. Tente novamente.';
      } finally {
        this.loading = false;
      }
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
}

.bg-left {
  position: fixed;
  left: 0;
  top: 0;
  height: 100vh;
  background: #000;
  transition: width 0.8s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 0;
}

.bg-right {
  position: fixed;
  right: 0;
  top: 0;
  height: 100vh;
  background: #fafafa;
  transition: width 0.8s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 0;
}

.auth-container {
  position: relative;
  z-index: 1;
  width: 100%;
  height: 100vh;
  display: flex;
}

.auth-section {
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: flex 0.8s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
}

.auth-section.active {
  flex: 6;
}

.auth-section.inactive {
  flex: 4;
}
 
.form-wrapper {
  width: 100%;
  max-width: 440px;
  padding: 0 40px;
}

.form-content {
  width: 100%;
  text-align: center;
}

.icon-box {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;
}

.login-section .icon-box {
  background: #fff;
  color: #000;
}

.cadastro-section .icon-box {
  background: #000;
  color: #fff;
}

.form-content h1 {
  font-size: 36px;
  font-weight: 600;
  margin-bottom: 8px;
  letter-spacing: -0.02em;
}

.login-section h1,
.login-section .subtitle {
  color: #fff;
}

.cadastro-section h1,
.cadastro-section .subtitle {
  color: #111827;
}

.subtitle {
  font-size: 16px;
  opacity: 0.7;
  margin-bottom: 32px;
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
}

.login-section label {
  color: rgba(255, 255, 255, 0.9);
}

.cadastro-section label {
  color: #374151;
}

.form-group input {
  width: 100%;
  padding: 14px 16px;
  border-radius: 10px;
  font-size: 15px;
  border: 1.5px solid;
  font-family: inherit;
  transition: all 0.2s;
}

.login-section input {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.2);
  color: #fff;
}

.login-section input::placeholder {
  color: rgba(255, 255, 255, 0.4);
}

.login-section input:focus {
  outline: none;
  background: rgba(255, 255, 255, 0.15);
  border-color: rgba(255, 255, 255, 0.4);
}

.cadastro-section input {
  background: #fff;
  border-color: #e5e7eb;
  color: #111827;
}

.cadastro-section input::placeholder {
  color: #9ca3af;
}

.cadastro-section input:focus {
  outline: none;
  border-color: #000;
}

.form-group small {
  display: block;
  font-size: 12px;
  margin-top: 6px;
  opacity: 0.6;
}

.login-section small {
  color: rgba(255, 255, 255, 0.6);
}

.cadastro-section small {
  color: #6b7280;
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
}

.login-section .btn-submit {
  background: #fff;
  color: #000;
}

.login-section .btn-submit:hover:not(:disabled) {
  background: #f0f0f0;
  transform: translateY(-1px);
}

.cadastro-section .btn-submit {
  background: #000;
  color: #fff;
}

.cadastro-section .btn-submit:hover:not(:disabled) {
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
  border: 2px solid;
  border-radius: 50%;
  border-top-color: transparent;
  animation: spin 0.6s linear infinite;
}

.login-section .spinner {
  border-color: #000;
  border-top-color: transparent;
}

.cadastro-section .spinner {
  border-color: #fff;
  border-top-color: transparent;
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

.alert-error {
  position: fixed;
  bottom: 24px;
  left: 70%;
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

.alert-error-login {
  position: fixed;
  bottom: 24px;
  left: 30%;
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

.alert-success {
  background: #f0fdf4;
  color: #166534;
  border: 1px solid #bbf7d0;
}

.footer-text {
  text-align: center;
  margin-top: 24px;
  font-size: 14px;
}

.login-section .footer-text {
  color: rgba(255, 255, 255, 0.7);
}

.cadastro-section .footer-text {
  color: #6b7280;
}

.footer-text a {
  font-weight: 600;
  cursor: pointer;
  margin-left: 4px;
  transition: opacity 0.2s;
}

.login-section .footer-text a {
  color: #fff;
}

.cadastro-section .footer-text a {
  color: #000;
}

.footer-text a:hover {
  opacity: 0.7;
}

.preview {
  text-align: center;
  padding: 40px;
}

.preview-icon {
  width: 96px;
  height: 96px;
  margin: 0 auto 24px;
  border-radius: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-section .preview-icon {
  background: rgba(255, 255, 255, 0.1);
  color: #fff;
}

.cadastro-section .preview-icon {
  background: rgba(0, 0, 0, 0.05);
  color: #000;
}

.preview h2 {
  font-size: 32px;
  font-weight: 600;
  margin-bottom: 8px;
}

.login-section .preview h2,
.login-section .preview p {
  color: #fff;
}

.cadastro-section .preview h2,
.cadastro-section .preview p {
  color: #111827;
}

.preview p {
  font-size: 16px;
  opacity: 0.7;
}

.fade-form-enter-active {
  transition: all 0.6s cubic-bezier(0.4, 0, 0.2, 1) 0.3s;
}

.fade-form-leave-active {
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.fade-form-enter-from {
  opacity: 0;
  transform: translateY(30px) scale(0.95);
}

.fade-form-leave-to {
  opacity: 0;
  transform: translateY(-20px) scale(0.98);
}

.fade-preview-enter-active {
  transition: all 0.6s cubic-bezier(0.4, 0, 0.2, 1) 0.4s;
}

.fade-preview-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.fade-preview-enter-from {
  opacity: 0;
  transform: scale(0.9);
}

.fade-preview-leave-to {
  opacity: 0;
  transform: scale(0.95);
}

@media (max-width: 768px) {
  .auth-container {
    flex-direction: column;
  }

  .bg-left,
  .bg-right {
    height: 50vh;
    width: 100% !important;
  }

  .bg-left {
    top: 0;
  }

  .bg-right {
    top: auto;
    bottom: 0;
  }

  .auth-section.active {
    flex: 7;
  }

  .auth-section.inactive {
    flex: 3;
  }

  .form-wrapper {
    padding: 0 24px;
  }

  .form-content h1 {
    font-size: 28px;
  }

  .alert {
    padding: 12px 14px;
    border-radius: 8px;
    font-size: 14px;
    margin-top: 16px;
    animation: slideDown 0.3s ease;
  }
}
</style>