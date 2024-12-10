import api from './api';
import { Vessel, VesselDetail, VesselRequest } from '../interfaces/vessel';

const BASE_URL = 'api/vessel';

export const getVessels = async (): Promise<Vessel[]> => {
    const response = await api.get(BASE_URL);
    return response.data; // Ensure this returns an array of Vessels
};

export const getVessel = async (id: string): Promise<VesselDetail> => {
    const response = await api.get(`${BASE_URL}/${id}`);
    return response.data;
};

export const createVessel = async (vessel: VesselRequest): Promise<Vessel> => {
    const response = await api.post(BASE_URL, vessel);
    return response.data;
};

export const updateVessel = async (id: string, vessel: VesselRequest): Promise<Vessel> => {
    const response = await api.put(`${BASE_URL}/${id}`, vessel);
    return response.data;
};

export const deleteVessel = async (id: string): Promise<void> => {
    await api.delete(`${BASE_URL}/${id}`);
}; 