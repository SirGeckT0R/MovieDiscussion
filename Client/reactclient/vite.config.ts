import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 6001,
    strictPort: true,
    host: true,
    origin: 'http://0.0.0.0:6001',
  },
});
