<template>
  <div class="form-group"> 
    
    <label v-if="label" :for="id">{{ label }}</label> 

    <div class="input-container">
      <input 
        :id="id"
        :type="input_type" 
        class="password-input-field"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
      />
      
      <button @click="obfuscateToggle" class="toggle-button" type="button">
        <i :class="eyeIconClasses"/> 
      </button>
    </div>

    <small v-if="hint">{{ hint }}</small> 
  </div>
</template>

<script>
export default {
  name: "PasswordToggleInput",
  emits: ['update:modelValue'],
  data() {
    return {
      input_type: this.type,
      passwordVisible: false,
      id: `password-input-${Math.random().toString(36).substring(2, 9)}`, 
    };
  },
  props: {
    modelValue: { type: String, default: '' }, 
    type: { type: String, default: "password" },
    label: { type: String, default: 'Senha' },
    placeholder: { type: String, default: '' },
    hint: { type: String, default: '' },
  },
  computed: {
    eyeIconClasses() {
      return {
        'fas': true,
        'fa-eye': this.passwordVisible, 
        'fa-eye-slash': !this.passwordVisible,
      };
    },
  },
  methods: {
    obfuscateToggle() {
      this.passwordVisible = !this.passwordVisible;
      this.input_type = this.passwordVisible ? "text" : "password";
    },
  },
};
</script>

<style scoped>
/* Estilos Base */
.form-group {
  margin-bottom: 20px;
  text-align: left;
}

/* Cor do Label no Dark Mode */
.form-group label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 8px;
  color: rgba(255, 255, 255, 0.9); /* Branco sutil */
}

/* Cor do Hint no Dark Mode */
.form-group small {
  display: block;
  font-size: 12px;
  margin-top: 6px;
  color: rgba(255, 255, 255, 0.6);
}

/* Estilo do container (Posicionamento) */
.input-container {
  position: relative;
  width: 82%;
}

/* ------------------------------------------- */
/* Estilo do Input para o Dark Mode (Ajustado) */
/* ------------------------------------------- */
.input-container .password-input-field {
  width: 100%;
  padding: 14px 45px 14px 16px; 
  
  /* Cores do Dark Mode */
  background-color: #242424; 
  color: rgba(255, 255, 255, 0.9); 
  
  /* === CORREÇÃO DA BORDA === */
  /* Remove a borda para igualar ao campo de e-mail da imagem */
  border: none; 
  /* Alternativa (se preferir manter 1px): */
  /* border: 1px solid #242424; */ 

  /* Raio de Borda (bem arredondado) */
  border-radius: 10px; 
  font-size: 15px;
  font-family: inherit;
  transition: all 0.2s;
  
  /* Estilo do placeholder */
  &::placeholder { 
    color: rgba(255, 255, 255, 0.4);
  }
}

/* Estilo e posicionamento do botão (Ícone do Olho) */
.toggle-button {
  position: absolute;
  top: 50%;
  right: 1px;
  left: 107%;
  transform: translateY(-50%);
  
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  /* Cor do ícone no Dark Mode */
  color: rgba(255, 255, 255, 0.6); 
  font-size: 14px;
  transition: color 0.2s;
}

.toggle-button:hover {
  color: rgba(255, 255, 255, 0.9); /* Cor ao passar o mouse */
}
/* Estilo do Input para o Dark Mode (Ajustado) */
.input-container .password-input-field {
  width: 100%;
  padding: 14px 45px 14px 16px; 
  
  /* Cores do Dark Mode */
  background-color: #242424; 
  color: rgba(255, 255, 255, 0.9); 
  
  /* === CORREÇÃO DA BORDA === */
  
  /* Mude esta linha: */
  /* border: none; */ /* << LINHA ANTIGA (Remover ou comentar) */

  /* Para esta linha (Adicionar): */
  border: 1.5px solid #545454; /* << Borda sutil do tema escuro */

  /* Raio de Borda (bem arredondado) */
  border-radius: 10px; 
  
  /* ... (O resto do CSS continua igual) ... */
  font-size: 15px;
  font-family: inherit;
  transition: all 0.2s;
  
  &::placeholder { 
    color: rgba(255, 255, 255, 0.4);
  }
}
</style>