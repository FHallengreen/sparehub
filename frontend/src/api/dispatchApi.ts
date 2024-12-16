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

export const fetchDestinations = async (destinationType: string): Promise<Supplier[] | Vessel[] | Warehouse[]> => {
  try {
    let endpoint = '';
    switch (destinationType) {
      case 'Warehouse':
        endpoint = '/api/warehouse';
        break;
      case 'Vessel':
        endpoint = '/api/vessel';
        break;
      case 'Supplier':
        endpoint = '/api/supplier';
        break;
      case 'Address':
        // Assume this is a placeholder for a future endpoint
        break;
      default:
        console.error(`Invalid destination type: ${destinationType}`);
        return [];
    }

    const response = await api.get(endpoint);
    console.log(`Fetched destinations for ${destinationType}:`, response.data);
    return response.data;
  } catch (error) {
    console.error(`Failed to fetch destination IDs for type: ${destinationType}`, error);
    return [];
  }
};

