import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

api.interceptors.request.use(
  (config) => {
    const session = localStorage.getItem('session');
    const token = session ? JSON.parse(session).token : null;

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('session');
      
      window.location.href = '/login';
    }

    return Promise.reject(error);
  }
);

export default api;
