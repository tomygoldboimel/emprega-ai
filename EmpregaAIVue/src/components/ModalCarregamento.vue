<template>
  <div v-if="isOpen" class="modal-overlay">
    <div class="modal-container">
      <h3 class="modal-title">Carregando Dados...</h3>

      <div class="loading-row">
        <div class="loading-icon-container">
          <img :src="loadingIcon" alt="Loading" class="loading-icon" />
        </div>
        <p class="modal-message">Por favor, aguarde...</p>
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
  flex-direction: column;
  align-items: flex-start;
  width: auto;
  min-width: 400px;
  height: 200px;
}

.modal-title {
  margin: 10px 0 20px 0;
  font-size: 1.8rem;
  font-weight: 600;
  color: #000;
  width: 100%;
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
@media (max-width: 768px) {
  .modal-container {
    min-width: 75%;
    height: auto;
    padding: 20px 25px;
    border-radius: 25px;
    align-items: center;
  }

  .modal-title {
    font-size: 1.4rem;
    text-align: center;
    margin-bottom: 15px;
  }

  .loading-row {
    gap: 20px;
    margin-right: 0;
    align-items: center;
  }

  .loading-icon {
    width: 60px;
    height: 60px;
  }

  .modal-message {
    font-size: 1.1rem;
    margin-top: 10px;
    margin-left: 0;
    text-align: center;
  }
}
</style>