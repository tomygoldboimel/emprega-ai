<template>
  <button 
    type="button"
    @click="$emit('toggle')"
    class="btn-microfone"
    :class="{ 'gravando': isRecording }"
    :title="isRecording ? 'Parar gravação' : 'Gravar por voz'"
  >
    <img 
      v-if="!isRecording"
      src="@/assets/icons/microphoneIcon.svg"
      alt="Microfone"
      width="18"
      height="18"
      class="microfone-icon"
    />
    <img 
      v-else
      src="@/assets/icons/recordingIcon.svg"
      alt="Gravando"
      width="10"
      height="10"
      class="icone-gravando"
    />
  </button>
</template>

<script>
export default {
  name: 'BotaoMicrofone',
  props: {
    isRecording: {
      type: Boolean,
      default: false
    }
  },
  emits: ['toggle']
}
</script>

<style scoped>
.btn-microfone {
  background: transparent;
  border: none;
  padding: 0; 
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;  
  height: 24px;
}

.microfone-icon{
  opacity: 0.3;
}

.btn-microfone:hover {
  background: rgba(0, 0, 0, 0);
}

.btn-microfone:hover svg {
  color: #dc2626;
}

.icone-gravando {
  margin-bottom: 35px;
  margin-left: 2px;
}

.btn-microfone:focus {
  outline: 2px solid #000;
  outline-offset: 2px;
}

.btn-microfone:active {
  transform: scale(0.95);
}
</style>