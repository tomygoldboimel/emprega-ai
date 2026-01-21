import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()], // ← REMOVEU o basicSsl()
  
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'https://localhost:7274',
        changeOrigin: true,
        secure: false,
      }
    }
  },
  
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})