import { fetchReviews } from '../api/reviewService';

export const getReviewsQuery = (id: string | undefined = '') => ({
  queryKey: ['reviews', id],
  queryFn: async () => await fetchReviews(id),
});
