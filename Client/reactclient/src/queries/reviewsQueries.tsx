import { fetchReviewByMovieAndUser, fetchReviews } from '../api/reviewService';

export const getReviewsQuery = (
  movieId: string | undefined = '',
  pageIndex: number
) => ({
  queryKey: ['reviews', `${movieId}${pageIndex}`],
  queryFn: async () => await fetchReviews(movieId, pageIndex),
});

export const getUserReviewForMovieQuery = (id: string | undefined = '') => ({
  queryKey: ['reviews', 'user' + id],
  queryFn: async () => await fetchReviewByMovieAndUser(id),
});
