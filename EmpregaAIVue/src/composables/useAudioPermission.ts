import { ref } from 'vue';

type PermissionStatus = 'prompt' | 'granted' | 'denied' | 'unknown';

export function useAudioPermission() {
  const permissionStatus = ref<PermissionStatus>('unknown');
  const isChecking = ref(false);

  const checkPermission = async (): Promise<PermissionStatus> => {
    isChecking.value = true;

    try {
      if ('permissions' in navigator) {
        const result = await navigator.permissions.query({ 
          name: 'microphone' as PermissionName 
        });
        
        permissionStatus.value = result.state as PermissionStatus;
        
        result.onchange = () => {
          permissionStatus.value = result.state as PermissionStatus;
        };
        
        return result.state as PermissionStatus;
      }
    } catch (error) {
      console.log('Permissions API não disponível, usando fallback');
    }

    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      
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

  const requestPermission = async (): Promise<boolean> => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      
      stream.getTracks().forEach(track => track.stop());
      
      permissionStatus.value = 'granted';
      return true;
    } catch (error: any) {
      
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