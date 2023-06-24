import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
    // vite 相关配置
    server: {
    port: 8776,
    host: true,
    open: true,
    proxy: {
      // https://cn.vitejs.dev/config/#server-proxy
      '/dev-api': {
        target: 'http://localhost:8777',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/dev-api/, '')
      },
      '/chathub': {
        target: 'http://localhost:8777',
        ws: true,
        rewrite: (path) => path.replace(/^\/chatHub/, '')
      }
    }
  }
})
