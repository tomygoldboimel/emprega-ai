<template>
  <div class="cadastro-wrapper">
    <div class="cadastro-container">
      <div class="cadastro-header">
        <div class="logo">
          <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
            <circle cx="9" cy="7" r="4"></circle>
            <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
          </svg>
        </div>
        <h1>Criar conta</h1>
        <p>Preencha os dados para começar</p>
      </div>

      <form @submit.prevent="handleCadastro" class="cadastro-form">
        <div class="form-group">
          <label for="nome">Nome completo</label>
          <input 
            type="text" 
            id="nome" 
            v-model="formUser.nome" 
            required
          />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input 
            type="email" 
            id="email" 
            v-model="formUser.email" 
            required 
          />
        </div>

        <div>
          <div class="form-group">
            <label for="password">Senha</label>
            <input 
              type="password" 
              id="password" 
              v-model="formUser.password" 
              placeholder="••••••••"
              required 
            />
            <small class="input-hint">Mínimo 6 caracteres</small>
          </div>
        </div>
        <div>
          <div class="form-group">
            <label for="confirmPassword">Confirmar</label>
            <input 
              type="password" 
              id="confirmPassword" 
              v-model="confirmPassword" 
              placeholder="••••••••"
              required 
            />
          </div>
        </div>

        <button type="submit" class="btn-primary" :disabled="loading">
          <span v-if="!loading">Criar Conta</span>
          <span v-else class="loading-spinner"></span>
        </button>

        <div v-if="errorMessage" class="alert alert-error">
          {{ errorMessage }}
        </div>

        <div v-if="successMessage" class="alert alert-success">
          {{ successMessage }}
        </div>
      </form>

      <div class="footer-link">
        <span>Já tem uma conta?</span>
        <router-link to="/login">Fazer login</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import usuarioService from '@/services/usuarioService';

export default {
  name: 'Cadastro',
  data() {
    return {
      formUser: {
        nome: '',
        email: '',
        password: '',
      },
      confirmPassword: '',
      errorMessage: '',
      successMessage: '',
      loading: false
    }
  },
  methods: {
    async handleCadastro() {
      this.errorMessage = '';
      this.successMessage = '';

      if (this.formUser.nome.length < 3) {
        this.errorMessage = 'Nome deve ter no mínimo 3 caracteres';
        return;
      }

      if (this.formUser.password.length < 6) {
        this.errorMessage = 'Senha deve ter no mínimo 6 caracteres';
        return;
      }

      if (this.formUser.password !== this.confirmPassword) {
        this.errorMessage = 'As senhas não conferem';
        return;
      }

      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(this.formUser.email)) {
        this.errorMessage = 'Email inválido';
        return;
      }

      this.loading = true;

      try {
        const novoUsuario = await usuarioService.adicionarUsuario({
          nome: this.formUser.nome,
          email: this.formUser.email,
          senha: this.formUser.password
        });

        this.successMessage = 'Cadastro realizado com sucesso!';
        localStorage.setItem('usuario', JSON.stringify(novoUsuario));

        setTimeout(() => {
          this.$router.push('/login');
        }, 100);

      } catch (error) {
        console.error('Erro ao cadastrar:', error);
        
        if (error.response) {
          this.errorMessage = error.response.data.message || 'Erro ao realizar cadastro';
        } else if (error.request) {
          this.errorMessage = 'Erro de conexão. Verifique se a API está rodando.';
        } else {
          this.errorMessage = 'Erro inesperado ao realizar cadastro';
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

.cadastro-wrapper {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fafafa;
  padding: 24px;
}

.cadastro-container {
  background: white;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  padding: 48px;
  width: 100%;
  max-width: 500px;
  animation: fadeIn 0.4s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.cadastro-header {
  text-align: center;
  margin-bottom: 32px;
}

.logo {
  width: 56px;
  height: 56px;
  background: #000;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
  color: white;
}

.cadastro-header h1 {
  font-size: 24px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 8px;
  letter-spacing: -0.02em;
}

.cadastro-header p {
  font-size: 14px;
  color: #6b7280;
  font-weight: 400;
}

.cadastro-form {
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 18px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-group label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  color: #374151;
  margin-bottom: 8px;
}

input {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 15px;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  font-family: inherit;
}

input:focus {
  outline: none;
  border-color: #000;
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.05);
}

input::placeholder {
  color: #9ca3af;
}

.input-hint {
  display: block;
  margin-top: 6px;
  font-size: 12px;
  color: #9ca3af;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background: #000;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-top: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 44px;
}

.btn-primary:hover:not(:disabled) {
  background: #1f2937;
}

.btn-primary:active:not(:disabled) {
  transform: scale(0.98);
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.loading-spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.alert {
  padding: 12px 14px;
  border-radius: 8px;
  font-size: 14px;
  margin-top: 16px;
  animation: slideDown 0.3s ease;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-4px);
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

.footer-link {
  text-align: center;
  padding-top: 24px;
  border-top: 1px solid #f3f4f6;
  font-size: 14px;
  color: #6b7280;
}

.footer-link span {
  margin-right: 6px;
}

.footer-link a {
  color: #000;
  text-decoration: none;
  font-weight: 500;
  transition: opacity 0.2s ease;
}

.footer-link a:hover {
  opacity: 0.7;
}

@media (max-width: 580px) {
  .cadastro-container {
    padding: 32px 24px;
  }
  
  .cadastro-header h1 {
    font-size: 22px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>