import api from './api';
import qs from "qs";

export const fetchOrders = async (searchTags: string[]): Promise<{ stockLocation: string }[]> => {
  const response = await api.get('/api/orders', {
    params: { searchTags },
    paramsSerializer: (params) => qs.stringify(params, { arrayFormat: 'repeat' }),
  });
  return response.data;
};


export const fetchOrderById = async (id: string) => {
  const response = await api.get(`/order/${id}`);
  return response.data;
};
