import qs from "qs";
import {Order} from "../interfaces/order.ts";
import {
    VesselOption,
    SupplierOption,
    Warehouse,
    Agent,
    OrderDetail,
    OrderRequest
  } from './../interfaces/order.ts';
import axios from 'axios';
import api from '../api/api.ts';

export const fetchOrders = async (searchTags: string[]): Promise<Order[]> => {
  const response = await api.get<Order[]>('/api/order', {
    params: { searchTerms: searchTags ?? [] },
    paramsSerializer: (params) => {
      return qs.stringify(params, { arrayFormat: 'repeat' });
    },
  });

  return response.data;
};

export const fetchVesselOptions = async (query: string): Promise<VesselOption[]> => {
  const response = await api.get<VesselOption[]>(`${import.meta.env.VITE_API_URL}/api/vessel/query`, {
    params: { searchQuery: query },
  });
  return response.data;
};

export const fetchSupplierOptions = async (query: string): Promise<SupplierOption[]> => {
  const response = await api.get<SupplierOption[]>(`${import.meta.env.VITE_API_URL}/api/supplier`, {
    params: { searchQuery: query },
  });
  return response.data;
};

export const fetchWarehouseOptions = async (query: string): Promise<Warehouse[]> => {
  const response = await api.get<Warehouse[]>(`${import.meta.env.VITE_API_URL}/api/warehouse`, {
    params: { searchQuery: query },
  });
  return response.data;
};

export const fetchAgentOptions = async (query: string): Promise<Agent[]> => {
  const response = await api.get<Agent[]>(`${import.meta.env.VITE_API_URL}/api/agent`, {
    params: { searchQuery: query },
  });
  return response.data;
};

export const fetchOrderById = async (id: string): Promise<OrderDetail | null> => {
  try {
    const response = await api.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.status === 404) {
      return null;
    }
    throw new Error('Failed to fetch order.');
  }
};

export const saveOrder = async (orderRequest: OrderRequest, isNewOrder: boolean, id?: string): Promise<OrderDetail> => {
  if (isNewOrder) {
    const response = await api.post<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order`, orderRequest);
    return response.data;
  } else {
    await api.put(`${import.meta.env.VITE_API_URL}/api/order/${id}`, orderRequest);
    const response = await api.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);
    return response.data;
  }
};

export const deleteOrderById = async (orderId: number): Promise<void> => {
  await api.delete(`${import.meta.env.VITE_API_URL}/api/order/${orderId}`);
};

export const fetchTrackingStatus = async (
  orderId: number | undefined,
  _transporter: string,
  trackingNumber: string
): Promise<{
  currentStep: string;
  statusDescription: string;
  location: string;
  timestamp: string;
  estimatedDelivery: string;
}> => {
  if (!orderId) {
    throw new Error('Order ID is required.');
  }
  const response = await api.get(
    `${import.meta.env.VITE_API_URL}/api/tracking/${trackingNumber}`
  );
  return response.data;
};
