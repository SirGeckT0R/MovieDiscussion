import { fetchDiscussions } from '../api/discussionsService';

export const getDiscussionsQuery = () => ({
  queryKey: ['discussions'],
  queryFn: async () => await fetchDiscussions(),
});
