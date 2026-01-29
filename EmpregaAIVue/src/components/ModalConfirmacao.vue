<template>
  <div v-if="isOpen" class="modal-overlay">
    <div class="modal-container">
      <div class="icon-wrapper">
        <div class="circle-check">
          <img :src="circleCheckIcon" alt="Check" class="check-icon" />
        </div>
      </div>

      <h2 class="modal-title">Ótimo</h2>

      <p class="modal-message">Cadastro de currículo realizado com sucesso</p>

      <button class="btn-ok" @click="$emit('confirmar')">
        Ok
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import circleCheckIcon from '@/assets/icons/circleCheckIcon.svg';
export default {
  props: {
    isOpen: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      circleCheckIcon
    };
  },
  emits: ['confirmar']
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
  border: 1px solid #000;
  border-radius: 40px;
  padding: 40px;
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 500px;
  max-width: 90%;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.icon-wrapper {
  margin-bottom: 15px;
}

.circle-check {
  width: 60px;
  height: 60px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.check-icon {
  width: 60px;
  height: 60px;
  animation: spin 1.2s linear infinite;
  padding-left: -20px;
}

.svg-icon {
  width: 35px;
  height: 35px;
}

.modal-title {
  font-size: 2.2rem;
  font-weight: 8600;
  margin: 0 0 15px 0;
  color: #000;
}

.modal-message {
  font-size: 1.2rem;
  color: #000;
  text-align: center;
  margin: 0 0 30px 0;
  font-weight: 500;
}

.btn-ok {
  background-color: #00802b;
  color: white;
  border: none;
  width: 250px;
  padding: 18px 0;
  border-radius: 25px;
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  transition: filter 0.2s;
}

.btn-ok:hover {
  filter: brightness(1.1);
}
</style>