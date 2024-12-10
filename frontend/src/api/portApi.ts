import { PortRequest } from '../interfaces/port';
import api from './api';
import { Port } from '../interfaces/port';

const BASE_URL = '/api/ports';

export const getPorts = async (): Promise<Port[]> => {
    const response = await api.get(BASE_URL);
    return response.data;
};

export const getPort = async (id: string) => {
    const response = await api.get(`${BASE_URL}/${id}`);
    return response.data;
};

export const createPort = async (port: PortRequest) => {
    const response = await api.post(BASE_URL, port);
    return response.data;
};

export const updatePort = async (id: string, port: PortRequest) => {
    const response = await api.put(`${BASE_URL}/${id}`, port);
    return response.data;
};

export const deletePort = async (id: string) => {
    const response = await api.delete(`${BASE_URL}/${id}`);
    return response.data;
}; 