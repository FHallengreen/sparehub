import api from './api';
import qs from "qs";
import {Order} from "../interfaces/order.ts";

export const fetchOrders = async (searchTags: string[]): Promise<Order[]> => {
  const response = await api.get<Order[]>('/api/order', {
    params: { searchTerms: searchTags ?? [] },
    paramsSerializer: (params) => {
      return qs.stringify(params, { arrayFormat: 'repeat' });
    },
  });

  return response.data;
};

export const fetchOrderById = async (id: string) => {
  const response = await api.get(`/order/${id}`);
  return response.data;
};
