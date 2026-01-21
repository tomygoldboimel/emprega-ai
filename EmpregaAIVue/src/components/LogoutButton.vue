<template>
  <button
    @click="$emit('logout')"
    class="btn-logout"
    type="button"
    title="Sair"
  >
    <img :src="logoutIcon" alt="Sair" />
  </button>
</template>

<script setup lang="ts">
import logoutIcon from '@/assets/icons/logoutIcon.svg'

// Define emits
defineEmits<{
  logout: []
}>()
</script>

<style scoped>
.btn-logout {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  /* Tamanho fixo para garantir o quadrado */
  width: 42px;
  height: 42px;
  padding: 0;
  
  /* Estilo do "Quadrado" */
  border: 1px solid #e5e7eb; /* Borda cinza suave */
  background-color: #ffffff; /* Fundo branco */
  border-radius: 8px;        /* Bordas arredondadas */
  
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-logout:hover {
  background-color: #f9fafb; /* Cinza bem claro no hover */
  border-color: #d1d5db;
}

/* Garante que o ícone fique centralizado e no tamanho certo */
.btn-logout img {
  width: 20px;
  height: 20px;
  display: block;
}
</style>