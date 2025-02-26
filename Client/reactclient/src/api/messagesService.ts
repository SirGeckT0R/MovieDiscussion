import { Message } from '../types/discussion';
import { axiosInstance } from './global';

export const fetchMessages = async (
  id: string | undefined
): Promise<Message[]> => {
  const messages: Message[] = await axiosInstance
    .get(`/api/messages/discussions/${id}`)
    .then((response) => {
      return response.data;
    });

  return messages;
};
