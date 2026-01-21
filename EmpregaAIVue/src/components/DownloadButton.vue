<template>
  <button
    @click="$emit('download')"
    class="btn-download"
    type="button"
    title="Download PDF"
  >
    <img :src="downloadIcon" alt="Download" />
  </button>
</template>

<script setup lang="ts">
import downloadIcon from '@/assets/icons/downloadIcon.svg'

// Define emits
defineEmits<{
  download: []
}>()
</script>

<style scoped>
.btn-download {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  /* Tamanho fixo para garantir o quadrado */
  width: 42px;
  height: 42px;
  padding: 0;
  
  /* Estilo do "Quadrado" */
  border: transparent; /* Borda cinza suave */
  background-color: #ffffff; /* Fundo branco */
  border-radius: 8px;        /* Bordas arredondadas */
  
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-download:hover {
  background-color: #f9fafb; /* Cinza bem claro no hover */
  border-color: #d1d5db;
}

/* Garante que o ícone fique centralizado e no tamanho certo */
.btn-download img {
  width: 20px;
  height: 20px;
  display: block;
}
</style>