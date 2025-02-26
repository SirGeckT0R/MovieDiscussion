import { CreateReviewRequest, PaginatedReview } from '../types/review';
import { axiosInstance } from './global';

export const fetchReviews = async (
  movieId: string,
  pageIndex: number = 1
): Promise<PaginatedReview> => {
  const reviews: PaginatedReview = await axiosInstance
    .get(
      `/api/reviews/movie?MovieId=${
        movieId ?? ''
      }&PageIndex=${pageIndex}&PageSize=2`
    )
    .then((response) => response.data);

  return reviews;
};

export const createReview = async (body: CreateReviewRequest) => {
  body.accountId = null;
  console.log(body);
  const response = await axiosInstance
    .post('/api/reviews', body)
    .then((response) => response.data);

  return response;
};
