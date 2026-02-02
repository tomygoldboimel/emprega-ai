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

defineEmits<{
  logout: []
}>()
</script>

<style scoped>
.btn-logout {
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

.btn-logout:hover {
  background-color: #f9fafb;
  border-color: #d1d5db;
}

.btn-logout img {
  width: 20px;
  height: 20px;
  display: block;
}
</style>