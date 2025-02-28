import {
  fetchDiscussion,
  fetchListOfDiscussions,
} from '../api/discussionsService';

export const getListOfDiscussionsQuery = () => ({
  queryKey: ['discussions'],
  queryFn: async () => await fetchListOfDiscussions(),
});

export const getDiscussionQuery = (id: string | undefined = '') => ({
  queryKey: ['discussions', id],
  queryFn: async () => await fetchDiscussion(id),
});
