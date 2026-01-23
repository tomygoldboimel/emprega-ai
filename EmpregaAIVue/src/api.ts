import axios from 'axios';

const api = axios.create({
  // A URL base deve ir até a raiz da sua API no Railway
  baseURL: 'https://emprega-ai-production.up.railway.app/api',
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true // Essencial se você usar Session/Cookies no .NET
});

export default api;