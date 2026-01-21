<template>
  <div v-if="isOpen" class="modal-overlay">
    <div class="modal-container">
      <h3 class="modal-title">Por favor aguarde...</h3>

      <div class="loading-row">
        <div class="loading-icon-container">
          <img :src="loadingIcon" alt="Loading" class="loading-icon" />
        </div>
        <p class="modal-message">Carregando...</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import loadingIcon from '@/assets/icons/loadingIcon.svg';

export default {
  props: {
    isOpen: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      loadingIcon
    };
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.modal-container {
  background: white;
  border-radius: 45px;
  padding: 35px 50px;
  display: flex;
  flex-direction: column; /* Força o empilhamento vertical das linhas */
  align-items: flex-start; /* Alinha o conteúdo à esquerda */
  width: auto;
  min-width: 400px;
  height: 200px;
}

.modal-title {
  margin: 10px 0 20px 0; /* Espaço abaixo do título para a próxima linha */
  font-size: 1.8rem;
  font-weight: 600;
  color: #000;
  width: 100%; /* Garante que ocupa a linha toda */
}

.loading-row {
  display: flex;
  flex-direction: row;
  align-items: space-between;
  gap: 50px;
  margin-right: 200px
}

.loading-icon-container {
  width: 70px;
  height: 70px;
}

.loading-icon {
  width: 120px;
  height: 120px;
  animation: spin 1.2s linear infinite;
  padding-left: -20px;
}

.modal-message {
  margin-top: 48px;
  margin-left: 20px;
  color: #000;
  font-size: 1.3rem;
  font-weight: 500;
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}
</style>