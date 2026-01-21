<template>
  <button
    @click="handleClick"
    class="btn-back"
    type="button"
    title="Voltar"
  >
    <img :src="ArrowIcon" alt="Voltar" class="icon-white" />
  </button>
</template>

<script setup lang="ts">
import ArrowIcon from '@/assets/icons/arrowIcon.svg'

const emit = defineEmits(['back'])

const handleClick = () => {
  emit('back')
}
</script>

<style scoped>
.btn-back {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 8px;
  border: none;
  background: transparent;
  color: inherit;
  cursor: pointer;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  transition: background-color 0.2s;
}

.btn-back:hover {
  background-color: rgba(0, 0, 0, 0.1);
}
.btn-back img {
  width: 16px;
  height: 16px;
  display: block;
  rotate: 180deg;
}
</style>