import api from './api';
import {Dispatch, DispatchDetail, DispatchRequest} from '../interfaces/dispatch';

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
  const response = await api.post<DispatchRequest>(`/api/dispatch`, dispatch);
  return response.data;
};

export const updateDispatch = async (id: string, dispatch: DispatchDetail): Promise<void> => {
  await api.put(`/api/dispatch/${id}`, dispatch);
};

export const deleteDispatch = async (id: number): Promise<void> => {
  await api.delete(`/api/dispatch/${id}`);
};
