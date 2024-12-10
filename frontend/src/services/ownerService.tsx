import api from './Api';

export const getOwners = async () => {
    return await api.get('/owner');
};

export const getOwnerById = async (ownerId: string) => {
    return await api.get(`/owner/${ownerId}`);
};

export const createOwner = async (ownerData: any) => {
    return await api.post('/owner', ownerData);
};

export const updateOwner = async (ownerId: string, ownerData: any) => {
    return await api.put(`/owner/${ownerId}`, ownerData);
};

export const deleteOwner = async (ownerId: string) => {
    return await api.delete(`/owner/${ownerId}`);
};
