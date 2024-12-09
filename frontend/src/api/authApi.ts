import api from './api.ts';
import {LoginResponse} from "../interfaces/order.ts";

export const login = async (email: string, password: string) => {
  const response = await api.post<LoginResponse>('/api/auth/login', { email, password });
  return response.data;
};

export const logout = () => {
  localStorage.removeItem('session');
  window.location.href = '/login';
};
