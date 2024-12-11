import api from './api';
import { Vessel, VesselDetail, VesselRequest } from '../interfaces/vessel';

const BASE_URL = '/api/vessel';

export const getVessels = async (): Promise<Vessel[]> => {
    const response = await api.get<Vessel[]>(BASE_URL);
    return response.data;
};

export const getVesselById = async (id: string): Promise<VesselDetail> => {
    const response = await api.get<VesselDetail>(`${BASE_URL}/${id}`);
    return response.data;
};

export const createVessel = async (vessel: VesselRequest) => {
    const response = await api.post(BASE_URL, vessel);
    return response.data;
};

export const updateVessel = async (id: string, vessel: VesselRequest) =>{
    console.log('Updating vessel:', vessel);
    await api.put<VesselRequest>(`${BASE_URL}/${id}`, vessel);
};

export const deleteVessel = async (id: string): Promise<void> => {
    await api.delete(`${BASE_URL}/${id}`);
};