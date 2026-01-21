<script setup lang="ts">
import { ref } from "vue";
import { useDropzone } from "vue3-dropzone";
import resumeUploadService from "@/services/resumeUploadService";

const loading = ref(false);
const error = ref<string | null>(null);

const emit = defineEmits<{
  (e: "success", data: any): void;
}>();

const onDrop = async (files: File[]) => {
  const file = files[0];
  if (!file) return;

  loading.value = true;
  error.value = null;

  try {
    const data = await resumeUploadService.uploadResume(file);

    // Validar estrutura dos dados
    if (!resumeUploadService.validarEstruturaProcessada(data)) {
      throw new Error("Estrutura de dados inválida retornada pelo servidor");
    }

    emit("success", data);
  } catch (e) {
    error.value = e instanceof Error ? e.message : "Erro ao enviar currículo. Tente novamente.";
  } finally {
    loading.value = false;
  }
};

const { getRootProps, getInputProps, isDragActive } = useDropzone({
  onDrop,
  accept: [
    "application/pdf",
    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
  ],
  maxFiles: 1,
  maxSize: 5 * 1024 * 1024, // 5MB
});
</script>

<template>
  <div
    v-bind="getRootProps()"
    class="dropzone"
    :class="{ active: isDragActive }"
  >
    <input v-bind="getInputProps()" />

    <div v-if="loading">
      <p>Enviando currículo...</p>
    </div>

    <div v-else>
      <p><strong>Arraste seu currículo aqui</strong></p>
      <p>ou clique para selecionar (PDF ou DOCX)</p>
    </div>

    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<style scoped>
.dropzone {
  border: 2px dashed #cfcfcf;
  padding: 32px;
  border-radius: 12px;
  text-align: center;
  cursor: pointer;
  background-color: #ffffff;
  transition: background-color 0.2s ease;
}

.dropzone.active {
  background-color: #f3f3f3;
}

.error {
  color: #d32f2f;
  margin-top: 12px;
}
</style>
