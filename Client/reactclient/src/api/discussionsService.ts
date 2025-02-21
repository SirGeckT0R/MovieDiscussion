import { Discussion } from '../types/discussion';
import { axiosInstance } from './global';

export const fetchDiscussions = async (): Promise<Discussion[]> => {
  const discussions: Discussion[] = await axiosInstance
    .get('/api/discussions')
    .then((response) => {
      return response.data;
    });

  return discussions;
};
