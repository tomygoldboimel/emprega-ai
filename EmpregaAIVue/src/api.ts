import axios from 'axios';

const api = axios.create({
  // O Vite escolhe a URL certa baseada no comando (npm run dev ou npm run build)
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true 
});

export default api;