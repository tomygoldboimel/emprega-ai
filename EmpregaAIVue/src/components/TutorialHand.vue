<template>
  <div v-if="visible" class="scene">
    <div
      class="hand-wrapper"
      :class="state"
      :style="{ opacity }"
      @animationend="onWrapperAnimationEnd"
    >
      <img
        :src="handSrc"
        class="hand"
        :class="{ tap: tapping }"
        @animationend="onTapEnd"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, watch, nextTick, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  modelValue: { type: Boolean, required: true },
  handSrc: { type: String, required: true },
  fadeDuration: { type: Number, default: 1200 },
  idleTime: { type: Number, default: 1000 }
})

const emit = defineEmits([
  'update:modelValue',
  'finished',
  'cancelled'
])


const visible = ref(false)
const state = ref('')
const tapping = ref(false)
const opacity = ref(1)

let idleTimer = null
let cancelled = false

const resetIdleTimer = () => {
  cancelled = true
  clearTimeout(idleTimer)

  if (visible.value) {
    visible.value = false
    emit('update:modelValue', false)
    emit('cancelled')
  }

  tapping.value = false
  opacity.value = 1
}

const startIdleTimer = () => {
  cancelled = false
  clearTimeout(idleTimer)

  idleTimer = setTimeout(() => {
    if (cancelled) return

    visible.value = true
    state.value = 'rise'
    opacity.value = 0.8
  }, props.idleTime)
}

watch(
  () => props.modelValue,
  (active) => {
    if (!active) {
      resetIdleTimer()
      return
    }

    startIdleTimer()
  }
)

const interactionEvents = [
  'click',
  'mousedown',
  'touchstart',
  'keydown'
]

onMounted(() => {
  interactionEvents.forEach(event =>
    window.addEventListener(event, resetIdleTimer, true)
  )
})

onBeforeUnmount(() => {
  clearTimeout(idleTimer)
  interactionEvents.forEach(event =>
    window.removeEventListener(event, resetIdleTimer, true)
  )
})

const onWrapperAnimationEnd = () => {
  if (cancelled) return
  tapping.value = true
}

const onTapEnd = async () => {
  if (cancelled) return

  tapping.value = false

  await nextTick()
  requestAnimationFrame(() => {
    opacity.value = 0
  })

  setTimeout(() => {
    visible.value = false
    emit('update:modelValue', false)
    emit('finished')
  }, props.fadeDuration)
}
</script>

<style scoped>
.scene {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 9999;
}

.hand-wrapper {
  position: absolute;
  left: 50%;
  bottom: 50%;
  transform: translateX(-50%);
  opacity: 0.7;
  transition: opacity 0.3s ease-out;
}

.hand-wrapper.rise {
  animation: riseUp 1s ease-out forwards;
}

.hand {
  width: 120px;
  display: block;
  transform-origin: center;
}

.hand.tap {
  animation: tap 0.25s ease forwards;
}

@keyframes riseUp {
  from { 
    bottom: -160px;
    left: 50%;
  }
  to   { 
    bottom: 65%;
    left: 52%;
  }
}

@keyframes tap {
  0%   { transform: scale(1); }
  40%  { transform: scale(0.85); }
  100% { transform: scale(1); }
}
</style>
