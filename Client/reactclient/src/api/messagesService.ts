import { Message } from '../types/discussion';
import { axiosInstance } from './global';

export const fetchMessages = async (id?: string): Promise<Message[]> => {
  const { data } = await axiosInstance.get(
    `/api/messages/discussions/${id ?? ''}`
  );

  return data;
};
