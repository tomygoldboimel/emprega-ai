<template>
  <button
    @click="handleClick"
    class="btn-save"
    type="button"
    title="Salvar Currículo"
    :disabled="loading"
  >
    <img v-if="!loading" :src="SaveIcon" alt="Salvar Currículo" class="icon-white" />
  </button>
</template>

<script setup lang="ts">
import SaveIcon from '@/assets/icons/saveIcon.svg'

withDefaults(defineProps<{
  loading?: boolean
}>(), {
  loading: false
})

const emit = defineEmits(['save'])

const handleClick = () => {
  emit('save')
}
</script>

<style scoped>
.btn-save {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 8px;
  border: none;
  background: #000;
  color: inherit;
  cursor: pointer;
  border-radius: 8px;
  border: 1px solid #000000;
  transition: background-color 0.2s;
}

.btn-save:hover:not(:disabled) {
  background-color: rgba(255, 255, 255, 0.1);
}

.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-save img {
  width: 42px;
  height: 20px;
  display: block;
  filter: invert(1) brightness(1.1);
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
