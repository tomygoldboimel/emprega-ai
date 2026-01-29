<template>
  <button
    @click="toggleActive"
    class="btn-audio-descricao"
    :class="{ ativo: isActive }"
    type="button"
    :disabled="disabled || loading"
  >
    <img v-if="!loading" :src="soundIcon" alt="Áudio descrição" />
    
    <span v-else class="loading-spinner"></span>
  </button>
</template>

<script setup lang="ts">
import soundIcon from '@/assets/icons/soundIcon.svg'
import { ref, watch } from 'vue'

// Recebe o estado inicial se necessário
const props = defineProps<{
  loading?: boolean
  disabled?: boolean
  active?: boolean
}>()

const emit = defineEmits<{
  toggle: [active: boolean]
}>()

const isActive = ref(props.active || false)

watch(() => props.active, (newValue) => {
  isActive.value = newValue || false
})

const toggleActive = () => {
  isActive.value = !isActive.value
  emit('toggle', isActive.value)
}
</script>

<style scoped>
  .btn-audio-descricao.tocando {
  animation: pulse-audio 1.5s ease-in-out infinite;
}

@keyframes pulse-audio {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); opacity: 0.8; }
}
.btn-audio-descricao {
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

.btn-audio-descricao:not(.ativo):not(:disabled) {
  animation: pulse-aura 2s 4;
}

.btn-audio-descricao:hover:not(:disabled) {
  background-color: #f9fafb;
  border-color: #d1d5db;
}

.btn-audio-descricao:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-audio-descricao.ativo {
  background-color: #000000;
  border-color: #000000;
}

.btn-audio-descricao.ativo:hover:not(:disabled) {
  background-color: #1a1a1a;
  border-color: #1a1a1a;
}

.btn-audio-descricao.ativo img {
  filter: brightness(0) invert(1);
}

.btn-audio-descricao img {
  width: 20px;
  height: 20px;
  display: block;
}

.loading-spinner {
  display: inline-block;
  width: 20px;
  height: 20px;
  border: 2px solid rgba(100, 100, 100, 0.3);
  border-top: 2px solid #646464;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

.btn-audio-descricao.ativo .loading-spinner {
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top: 2px solid #ffffff;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
@keyframes pulse-aura {
  0% {
    box-shadow: 0 0 0 0 rgba(0, 0, 0, 0.2);
  }
  70% {
    box-shadow: 0 0 0 10px rgba(0, 0, 0, 0);
  }
  100% {
    box-shadow: 0 0 0 0 rgba(0, 0, 0, 0);
  }
}
</style>
