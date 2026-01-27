import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()], // ← REMOVEU o basicSsl()
  
  server: {
    port: 5173,
    allowedHosts: [
      'dante-fibular-pulpily.ngrok-free.dev' // Adicione exatamente o link do seu ngrok aqui
    ],
    proxy: {
      
      '/api': {
        target: '192.168.1.101:7274',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  },
  
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})