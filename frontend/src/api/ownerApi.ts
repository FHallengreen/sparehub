import api from './api';
import { Owner, OwnerDetail, OwnerRequest } from '../interfaces/owner';



export const getOwners = async (): Promise<Owner[]> => {
    const response = await api.get<Owner[]>('api/owner');
    return response.data;
};

export const getOwner = async (id: string): Promise<OwnerDetail> => {
    const response = await api.get<OwnerDetail>(`api/owner/${id}`);
    return response.data;
};

export const createOwner = async (owner: OwnerRequest): Promise<OwnerRequest> => {
    const response = await api.post<OwnerRequest>('api/owner', owner);
    return response.data;
};

export const updateOwner = async (id: string, owner: OwnerDetail): Promise<void> => {
    await api.put(`api/owner/${id}`, owner);
};

export const deleteOwner = async (id: string): Promise<void> => {
    await api.delete(`api/owner/${id}`);
};
