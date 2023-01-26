import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  base: '/dist/',
  build: {
    outDir: '../wwwroot/dist',
    emptyOutDir: true,
    manifest: true,
    rollupOptions: {
      input: {
        main: './src/main.tsx'
      }
    }
  },
  plugins: [
    react()
  ]
})