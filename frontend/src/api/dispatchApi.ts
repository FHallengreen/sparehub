import api from './api';
import {DispatchDetail, DispatchRequest} from '../interfaces/dispatch';

export const getDispatch = async (id: string): Promise<DispatchDetail> => {
  const response = await api.get<DispatchDetail>(`/api/dispatch/${id}`);
  return response.data;
};

export const createDispatch = async (dispatch: DispatchRequest): Promise<DispatchRequest> => {
  const response = await api.post<DispatchRequest>(`/api/dispatch`, dispatch);
  return response.data;
};

export const updateDispatch = async (id: string, dispatch: DispatchDetail): Promise<void> => {
  await api.put(`/api/dispatch/${id}`, dispatch);
};

export const deleteDispatch = async (id: number): Promise<void> => {
  await api.delete(`/api/dispatch/${id}`);
};
