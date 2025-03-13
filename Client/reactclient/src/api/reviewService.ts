import {
  CreateReviewRequest,
  DeleteReviewRequest,
  PaginatedReview,
  Review,
  UpdateReviewRequest,
} from '../types/review';
import { axiosInstance } from './global';

export const fetchReviews = async (
  movieId?: string,
  pageIndex: number = 1
): Promise<PaginatedReview> => {
  const { data } = await axiosInstance.get(
    `/api/reviews/movie?MovieId=${
      movieId ?? ''
    }&PageIndex=${pageIndex}&PageSize=2`
  );

  return data;
};

export const fetchReviewByMovieAndUser = async (
  movieId: string
): Promise<Review> => {
  const { data } = await axiosInstance.get(`/api/reviews?MovieId=${movieId}`);

  return data;
};

export const createReview = async (body: CreateReviewRequest) => {
  body.accountId = null;
  const { data } = await axiosInstance.post('/api/reviews', body);

  return data;
};

export const updateReview = async (body: UpdateReviewRequest) => {
  const { data } = await axiosInstance.put(`/api/reviews/${body.id}`, body);

  return data;
};

export const deleteReview = async (body: DeleteReviewRequest) => {
  const { data } = await axiosInstance.delete(`/api/reviews/${body.id}`);

  return data;
};
