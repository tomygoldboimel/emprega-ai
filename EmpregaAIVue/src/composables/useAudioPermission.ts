// src/composables/useAudioPermission.ts
import { ref } from 'vue';

type PermissionStatus = 'prompt' | 'granted' | 'denied' | 'unknown';

export function useAudioPermission() {
  const permissionStatus = ref<PermissionStatus>('unknown');
  const isChecking = ref(false);

  // Verificar status da permissão
  const checkPermission = async (): Promise<PermissionStatus> => {
    isChecking.value = true;

    try {
      // Tentar verificar permissão via Permissions API
      if ('permissions' in navigator) {
        const result = await navigator.permissions.query({ 
          name: 'microphone' as PermissionName 
        });
        
        permissionStatus.value = result.state as PermissionStatus;
        
        // Escutar mudanças
        result.onchange = () => {
          permissionStatus.value = result.state as PermissionStatus;
        };
        
        return result.state as PermissionStatus;
      }
    } catch (error) {
      console.log('Permissions API não disponível, usando fallback');
    }

    // Fallback: tentar acessar microfone
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      
      // Parar stream imediatamente
      stream.getTracks().forEach(track => track.stop());
      
      permissionStatus.value = 'granted';
      return 'granted';
    } catch (error: any) {
      if (error.name === 'NotAllowedError') {
        permissionStatus.value = 'denied';
        return 'denied';
      } else {
        permissionStatus.value = 'prompt';
        return 'prompt';
      }
    } finally {
      isChecking.value = false;
    }
  };

  // Solicitar permissão
  const requestPermission = async (): Promise<boolean> => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      
      // Parar stream
      stream.getTracks().forEach(track => track.stop());
      
      permissionStatus.value = 'granted';
      return true;
    } catch (error: any) {
      console.error('Erro ao solicitar permissão:', error);
      
      if (error.name === 'NotAllowedError') {
        permissionStatus.value = 'denied';
      }
      
      return false;
    }
  };

  return {
    permissionStatus,
    isChecking,
    checkPermission,
    requestPermission,
  };
}