import { fetchMessages } from '../api/messagesService';

export const getMessagesQuery = (id: string | undefined = '') => ({
  queryKey: ['messages', id],
  queryFn: async () => await fetchMessages(id),
});
