import api from './api';
import { Owner, OwnerRequest } from '../interfaces/owner';

const BASE_URL = 'api/owner';

export const getOwners = async (): Promise<Owner[]> => {
    const response = await api.get(BASE_URL);
    return response.data;
};

export const getOwnerById = async (id: string): Promise<Owner> => {
    const response = await api.get(`${BASE_URL}/${id}`);
    return response.data;
};

export const createOwner = async (owner: OwnerRequest): Promise<Owner> => {
    const response = await api.post(BASE_URL, owner);
    return response.data;
};

export const updateOwner = async (id: string, owner: OwnerRequest): Promise<Owner> => {
    const response = await api.put(`${BASE_URL}/${id}`, owner);
    return response.data;
};

export const deleteOwner = async (id: string): Promise<void> => {
    await api.delete(`${BASE_URL}/${id}`);
};
