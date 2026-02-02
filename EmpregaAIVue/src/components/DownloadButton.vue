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

defineEmits<{
  download: []
}>()
</script>

<style scoped>
.btn-download {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 42px;
  height: 42px;
  padding: 0;
  
  border: transparent;
  background-color: #ffffff;
  border-radius: 8px;
  
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-download:hover {
  background-color: #f9fafb;
  border-color: #d1d5db;
}

.btn-download img {
  width: 20px;
  height: 20px;
  display: block;
}
</style>