<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ContrastIcon from '@/assets/icons/contrastIcon.svg'

const isHighContrast = ref(false)

const toggleContrast = () => {
  isHighContrast.value = !isHighContrast.value
  const root = document.documentElement

  if (isHighContrast.value) {
    root.classList.add('high-contrast')
    localStorage.setItem('contraste', 'true')
  } else {
    root.classList.remove('high-contrast')
    localStorage.setItem('contraste', 'false')
  }
}

onMounted(() => {
  const saved = localStorage.getItem('contraste')
  if (saved === 'true') {
    isHighContrast.value = true
    document.documentElement.classList.add('high-contrast')
  }
})
</script>

<template>
  <button
    type="button"
    class="btn-contrast"
    :class="{ ativo: isHighContrast }"
    @click="toggleContrast"
    :aria-pressed="isHighContrast"
    aria-label="Alternar modo de alto contraste"
    title="Alternar Alto Contraste"
  >
    <img
      :src="ContrastIcon"
      alt=""
      aria-hidden="true"
    />
  </button>
</template>

<style scoped>
.btn-contrast {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 42px;
  height: 42px;
  padding: 0;
  border: 1px solid #e5e7eb;
  background-color: #ffffff;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-contrast:hover:not(:disabled) {
  background-color: #f9fafb;
  border-color: #d1d5db;
}

.btn-contrast:focus-visible {
  outline: 2px solid #2563eb;
  outline-offset: 2px;
}

.btn-contrast:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-contrast.ativo {
  background-color: #000000;
  border-color: #000000;
}

.btn-contrast.ativo:hover:not(:disabled) {
  background-color: #1a1a1a;
  border-color: #1a1a1a;
}

.btn-contrast img {
  width: 20px;
  height: 20px;
  display: block;
}

.btn-contrast.ativo img {
  filter: brightness(0) invert(1);
}
</style>
