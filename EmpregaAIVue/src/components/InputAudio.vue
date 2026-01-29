<template>
  <div class="voice-input">
    <input
      v-model="localValue"
      :type="type"
      :placeholder="placeholder"
      :required="required"
      class="voice-input__field"
      @input="$emit('update:modelValue', localValue)"
    />

    <button
      type="button"
      @click="toggleRecording"
      class="voice-input__btn"
      :class="{ 'voice-input__btn--recording': isListening }"
      :disabled="!isSupported"
      :title="isSupported ? 'Gravar por voz' : 'Seu navegador não suporta reconhecimento de voz'"
    >
    </button>

    <div v-if="error" class="voice-input__error">
      {{ error }}
    </div>

    <div v-if="interimTranscript && isListening" class="voice-input__interim">
      {{ interimTranscript }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { useSpeechRecognition } from '@/composables/useSpeechRecognition';

interface Props {
  modelValue: string;
  placeholder?: string;
  type?: string;
  required?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  placeholder: '',
  type: 'text',
  required: false,
});

const emit = defineEmits<{
  'update:modelValue': [value: string];
}>();

const localValue = ref(props.modelValue);

const {
  isSupported,
  isListening,
  transcript,
  interimTranscript,
  error,
  startRecording,
  stopRecording,
} = useSpeechRecognition();

watch(() => props.modelValue, (newValue) => {
  localValue.value = newValue;
});

watch(transcript, (newTranscript) => {
  if (newTranscript) {
    localValue.value = newTranscript;
    emit('update:modelValue', newTranscript);
  }
});

const toggleRecording = () => {
  if (isListening.value) {
    stopRecording();
  } else {
    startRecording();
  }
};
</script>

<style scoped>
.voice-input {
  position: relative;
}

.voice-input__field {
  width: 100%;
  padding: 1rem;
  padding-right: 3.5rem;
  font-size: 1.1rem;
  border: 2px solid #ddd;
  border-radius: 8px;
  transition: all 0.3s;
}

.voice-input__field:focus {
  outline: none;
  border-color: #1351B4;
  box-shadow: 0 0 0 3px rgba(19, 81, 180, 0.1);
}

.voice-input__btn {
  position: absolute;
  right: 0.5rem;
  top: 50%;
  transform: translateY(-50%);
  background: #f0f5ff;
  border: 2px solid #1976d2;
  border-radius: 50%;
  width: 2.5rem;
  height: 2.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  font-size: 1.25rem;
  transition: all 0.3s;
}

.voice-input__btn:hover:not(:disabled) {
  background: #e3f2fd;
  transform: translateY(-50%) scale(1.1);
}

.voice-input__btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.voice-input__btn--recording {
  background: #ff5252;
  border-color: #ff5252;
  animation: pulse-recording 1.5s infinite;
}

.voice-input__error {
  margin-top: 0.5rem;
  padding: 0.75rem;
  background: #ffebee;
  border: 1px solid #ef5350;
  border-radius: 6px;
  color: #c62828;
  font-size: 0.9rem;
}

.voice-input__interim {
  margin-top: 0.5rem;
  padding: 0.75rem;
  background: #e3f2fd;
  border: 1px dashed #1976d2;
  border-radius: 6px;
  color: #1565c0;
  font-size: 0.95rem;
  font-style: italic;
}

@keyframes pulse-recording {
  0%, 100% {
    transform: translateY(-50%) scale(1);
    box-shadow: 0 0 0 0 rgba(255, 82, 82, 0.7);
  }
  50% {
    transform: translateY(-50%) scale(1.05);
    box-shadow: 0 0 0 10px rgba(255, 82, 82, 0);
  }
}
</style>