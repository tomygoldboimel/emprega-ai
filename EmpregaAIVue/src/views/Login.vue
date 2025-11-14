<template>
  <div class="auth-page">
    <!-- Background animado -->
    <div 
      class="bg-left" 
      :style="{ width: activeMode === 'login' ? '60%' : '40%' }"
    ></div>
    <div 
      class="bg-right" 
      :style="{ width: activeMode === 'cadastro' ? '60%' : '40%' }"
    ></div>

    <!-- Container principal -->
    <div class="auth-container">
      <!-- SEÇÃO LOGIN -->
      <div 
        :class="['auth-section', 'login-section', { active: activeMode === 'login', inactive: activeMode !== 'login' }]"
      >
        <!-- Formulário Login -->
        <transition name="fade-form">
          <div v-if="activeMode === 'login'" class="form-wrapper" key="login-form">
            <div class="form-content">
              <div class="icon-box">
                <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                  <circle cx="12" cy="7" r="4"></circle>
                </svg>
              </div>
              
              <h1>Bem-vindo</h1>
              <p class="subtitle">Entre com sua conta</p>

              <div class="form-group">
                <label>Email</label>
                <input 
                  type="email" 
                  v-model="loginEmail"
                  placeholder="seu@email.com"
                  @keyup.enter="handleLogin"
                />
              </div>

              <div class="form-group">
                <PasswordInputEscuro type="password" 
                placeholder="••••••••"
                v-model="loginPassword"/>
              </div>

              <button 
                class="btn-submit" 
                @click="handleLogin"
                :disabled="loading"
              >
                <span v-if="!loading">Entrar</span>
                <div v-else class="spinner"></div>
              </button>

              <div v-if="loginError" class="alert-error-login">
                {{ loginError }}
              </div>

              <div class="footer-text">
                <span>Não tem uma conta?</span>
                <a @click="changeMode('cadastro')">Cadastre-se</a>
              </div>
            </div>
          </div>
        </transition>

        <!-- Preview Login -->
        <transition name="fade-preview">
          <div v-if="activeMode !== 'login'" class="preview" key="login-preview">
            <!-- <div class="preview-icon">
              <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                <circle cx="12" cy="7" r="4"></circle>
              </svg>
            </div>
            <h2>Bem-vindo</h2>
            <p>Entre com sua conta</p> -->
          </div>
        </transition>
      </div>

      <!-- SEÇÃO CADASTRO -->
      <div 
        :class="['auth-section', 'cadastro-section', { active: activeMode === 'cadastro', inactive: activeMode !== 'cadastro' }]"
      >
        <!-- Formulário Cadastro -->
        <transition name="fade-form">
          <div v-if="activeMode === 'cadastro'" class="form-wrapper" key="cadastro-form">
            <div class="form-content">
              <div class="icon-box">
                <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
                  <circle cx="9" cy="7" r="4"></circle>
                  <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
                  <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
                </svg>
              </div>
              
              <h1>Criar conta</h1>
              <p class="subtitle">Preencha os dados para começar</p>

              <div class="form-group">
                <label>Nome completo</label>
                <input 
                  type="text" 
                  v-model="cadastroNome"
                />
              </div>

              <div class="form-group">
                <label>Email</label>
                <input 
                  type="email" 
                  v-model="cadastroEmail"
                />
              </div>

              <div class="form-group">
                <label>Senha</label>
                <input 
                  type="password" 
                  v-model="cadastroPassword"
                  placeholder="••••••••"
                />
                <small>Mínimo 6 caracteres</small>
              </div>

              <div class="form-group">
                <label>Confirmar</label>
                <input 
                  type="password" 
                  v-model="cadastroConfirmPassword"
                  placeholder="••••••••"
                  @keyup.enter="handleCadastro"
                />
              </div>

              <button 
                class="btn-submit" 
                @click="handleCadastro"
                :disabled="loading"
              >
                <span v-if="!loading">Criar Conta</span>
                <div v-else class="spinner"></div>
              </button>

              <div v-if="cadastroError" class="alert-error">
                {{ cadastroError }}
              </div>

              <div v-if="cadastroSuccess" class="alert-success">
                {{ cadastroSuccess }}
              </div>

              <div class="footer-text">
                <span>Já tem uma conta?</span>
                <a @click="changeMode('login')">Fazer login</a>
              </div>
            </div>
          </div>
        </transition>

        <!-- Preview Cadastro -->
        <transition name="fade-preview">
          <div v-if="activeMode !== 'cadastro'" class="preview" key="cadastro-preview">
            <!-- <div class="preview-icon">
              <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
                <circle cx="9" cy="7" r="4"></circle>
                <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
                <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
              </svg>
            </div>
            <h2>Criar conta</h2>
            <p>Preencha os dados para começar</p> -->
          </div>
        </transition>
      </div>
    </div>
    <div v-if="errorMessage" class="alert alert-error">
        {{ errorMessage }}
      </div>
  </div>
</template>

<script>
import usuarioService from '@/services/usuarioService';
import curriculoService from '@/services/curriculoService';
import PasswordInputEscuro from '@/components/PasswordInputEscuro.vue';

export default {
  name: 'AuthUnified',
  components: {
    PasswordInputEscuro,
  },
  data() {
    return {
      activeMode: 'login',
      loading: false,
      
      loginEmail: '',
      loginPassword: '',
      loginError: '',
      errorMessage: '',
      cadastroNome: '',
      cadastroEmail: '',
      cadastroPassword: '',
      cadastroConfirmPassword: '',
      cadastroError: '',
      cadastroSuccess: ''
    }
  },
  methods: {
    changeMode(mode) {
      this.activeMode = mode;
      this.loginError = '';
      this.cadastroError = '';
      this.cadastroSuccess = '';
    },

    async handleLogin() {
      this.loginError = '';
      
      if (!this.loginEmail || !this.loginPassword) {
        this.loginError = 'Preencha todos os campos';
        return;
      }

      this.loading = true;
      
      try {
        const loginValido = await usuarioService.login(this.loginEmail, this.loginPassword);
        
        if (loginValido) {
          // Busca o usuário completo
          const usuarios = await usuarioService.listarUsuarios();
          const usuario = usuarios.find(u => u.email === this.loginEmail);
          
          if (usuario) {
            const usuarioParaSalvar = {
              id: usuario.id,
              email: usuario.email
            };
            
            localStorage.setItem('usuario', JSON.stringify(usuarioParaSalvar));
            
            // Verifica se tem currículo usando o service
            try {
              const curriculos = await curriculoService.listarCurriculosPorUsuario(usuario.id);
              console.log(curriculos)
              if (curriculos) {
                console.log("AAAAAA")
                this.$router.push(`/curriculo/visualizar/${curriculos.id}`);
              } else {
                // NÃO TEM CURRÍCULO - Vai para criar
                this.$router.push('/curriculo');
              }
            } catch (error) {
              console.error('Erro ao verificar currículo:', error);
              // Em caso de erro, vai para criar
              this.$router.push('/curriculo');
            }
          }
        } else {
          this.loginError = 'Email ou senha inválidos';
        }
      } catch (error) {
        console.error('Erro ao fazer login:', error);
        this.loginError = 'Erro ao fazer login. Tente novamente.';
      } finally {
        this.loading = false;
      }
    },

    async handleCadastro() {
      this.cadastroError = '';
      this.cadastroSuccess = '';

      if (this.cadastroNome.length < 3) {
        this.cadastroError = 'Nome deve ter no mínimo 3 caracteres';
        return;
      }

      if (this.cadastroPassword.length < 6) {
        this.cadastroError = 'Senha deve ter no mínimo 6 caracteres';
        return;
      }

      if (this.cadastroPassword !== this.cadastroConfirmPassword) {
        this.cadastroError = 'As senhas não conferem';
        return;
      }

      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(this.cadastroEmail)) {
        this.cadastroError = 'Email inválido';
        return;
      }

      this.loading = true;

      try {
        const novoUsuario = await usuarioService.adicionarUsuario({
          nome: this.cadastroNome,
          email: this.cadastroEmail,
          senha: this.cadastroPassword
        });
        this.cadastroSuccess = 'Cadastro realizado com sucesso!';
        localStorage.setItem('usuario', JSON.stringify(novoUsuario));

        setTimeout(() => {
          this.changeMode('login');
          this.cadastroNome = '';
          this.cadastroEmail = '';
          this.cadastroPassword = '';
          this.cadastroConfirmPassword = '';
        }, 1500);

      } catch (error) {
        console.error('Erro ao cadastrar:', error);
        if (error.response?.status === 400){
          this.errorMessage = 'E-mail já cadastrado. Redirecionando ao Login'
          setTimeout(() => {
            this.errorMessage = '';
            this.cadastroNome = '';
            this.cadastroEmail = '';
            this.cadastroPassword = '';
            this.cadastroConfirmPassword = '';
            this.changeMode('login');
          }, 2000);
        }
        else if (error.response) {
          this.cadastroError = error.response.data.message || 'Erro ao realizar cadastro';
        } else if (error.request) {
          this.cadastroError = 'Erro de conexão. Verifique se a API está rodando.';
        } else {
          this.cadastroError = 'Erro inesperado ao realizar cadastro';
        }
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

/* BACKGROUNDS ANIMADOS */
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

/* CONTAINER */
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

/* FORMULÁRIOS */
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
  margin-bottom: 24px;
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

/* BOTÕES */
.btn-submit {
  width: 100%;
  padding: 14px;
  border-radius: 10px;
  font-size: 16px;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
  margin-top: 8px;
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

/* ALERTAS */
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

.alert-error-login{
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

/* FOOTER TEXT */
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

/* PREVIEW */
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

/* TRANSIÇÕES */
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

/* RESPONSIVO */
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