import api from './api';
import { VesselAtPort, VesselAtPortDetail, VesselAtPortRequest } from '../interfaces/vesselAtPort';

const BASE_URL = '/api/vessel-at-port';

export const getVesselsAtPorts = async (): Promise<VesselAtPort[]> => {
    const response = await api.get<VesselAtPort[]>(BASE_URL);
    return response.data;
};

export const getVesselAtPort = async (vesselId: string): Promise<VesselAtPortDetail> => {
    const response = await api.get<VesselAtPortDetail>(`${BASE_URL}/vessel/${vesselId}`);
    return response.data;
};

export const createVesselAtPort = async (vesselAtPort: VesselAtPortRequest): Promise<VesselAtPort> => {
    const response = await api.post(BASE_URL, vesselAtPort);
    return response.data;
};

export const updateVesselAtPort = async (id: string, vesselAtPort: VesselAtPortRequest): Promise<VesselAtPort> => {
    const response = await api.put(`${BASE_URL}/${id}`, vesselAtPort);
    return response.data;
};

export const deleteVesselAtPort = async (id: string): Promise<void> => {
    await api.delete(`${BASE_URL}/${id}`);
};