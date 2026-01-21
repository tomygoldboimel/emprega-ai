<template>
  <div class="app">
    <h1>Exemplo de uso do TutorialHand</h1>

    <button @click="ativarAudio">
      {{ audioAtivo ? '🔊 Ouvindo...' : '🔈 Ativar Áudio' }}
    </button>

    <TutorialHand
      v-model="mostrarTutorial"
      :hand-src="pointerHand"
      :idle-time="1000"
      @finished="onTutorialFinished"
      @cancelled="onTutorialCancelled"
    />

  </div>
</template>

<script setup>
import { ref } from 'vue'
import TutorialHand from './components/TutorialHand.vue'
import pointerHand from './assets/pointerHand.svg'

const audioAtivo = ref(false)
const mostrarTutorial = ref(false)

const ativarAudio = () => {
  audioAtivo.value = true
  mostrarTutorial.value = true
}

const onTutorialFinished = () => {
  audioAtivo.value = false
  mostrarTutorial.value = false

}

const onTutorialCancelled = () => {
  audioAtivo.value = false
  mostrarTutorial.value = false

  console.log('Tutorial cancelado pelo usuário')
}

</script>

<style>
.app {
  padding: 40px;
  font-family: sans-serif;
}

button {
  margin-top: 20px;
  padding: 12px 24px;
  border-radius: 20px;
  border: none;
  background: #007bff;
  color: white;
  cursor: pointer;
}
</style>
