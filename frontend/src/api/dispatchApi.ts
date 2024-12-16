import api from './api';
import {Dispatch, DispatchDetail, DispatchRequest} from '../interfaces/dispatch';
import {Supplier} from "../interfaces/supplier.ts";
import {Warehouse} from "../interfaces/warehouse.ts";
import {Vessel} from "../interfaces/vessel.ts";

export const getDispatches = async (tags: string[]): Promise<Dispatch[]> => {
  const response = await api.get<Dispatch[]>('/api/dispatch', {
    params: { searchTerms: tags ?? [] },
  });

  return response.data;
}

export const getDispatch = async (id: string): Promise<DispatchDetail> => {
  const response = await api.get<DispatchDetail>(`/api/dispatch/${id}`);
  console.log(response.data);
  return response.data;
};

export const createDispatch = async (dispatch: DispatchRequest): Promise<DispatchRequest> => {
  console.log(dispatch);
  const response = await api.post<DispatchRequest>(`/api/dispatch`, dispatch);
  return response.data;
};

export const updateDispatch = async (id: string, dispatch: DispatchDetail): Promise<void> => {
  await api.put(`/api/dispatch/${id}`, dispatch);
};

export const deleteDispatch = async (id: number): Promise<void> => {
  await api.delete(`/api/dispatch/${id}`);
};

export const fetchOrigins = async (originType: string): Promise<Supplier[] | Warehouse[]> => {
  try {
    let endpoint = '';
    if (originType.toLowerCase() === 'warehouse') {
      endpoint = '/api/warehouse';
    } else if (originType.toLowerCase() === 'supplier') {
      endpoint = '/api/supplier';
    } else {
      console.error(`Invalid origin type: ${originType}`);
      return [];
    }

    const response = await api.get(endpoint);
    // Assuming the data contains an array of objects with an `id` property
    return response.data;
  } catch (error) {
    console.error(`Failed to fetch origin IDs for type: ${originType}`, error);
    return [];
  }
};

export const fetchDestinations = async (destinationType: string): Promise<Supplier[] | Vessel[]> => {
  try {
    let endpoint = '';
    if (destinationType.toLowerCase() === 'warehouse') {
      endpoint = '/api/warehouse';
    } else if (destinationType.toLowerCase() === 'vessel') {
      // Replace with the correct endpoint for vessels if available
      endpoint = '/api/vessel'; // Adjust as necessary
    } else {
      console.error(`Invalid destination type: ${destinationType}`);
      return [];
    }

    const response = await api.get(endpoint);
    // Assuming the data contains an array of objects with an `id` property
    return response.data;
  } catch (error) {
    console.error(`Failed to fetch destination IDs for type: ${destinationType}`, error);
    return [];
  }
};

