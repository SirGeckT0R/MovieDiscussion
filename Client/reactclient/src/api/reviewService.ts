import {
  CreateReviewRequest,
  PaginatedReview,
  Review,
  UpdateReviewRequest,
} from '../types/review';
import { axiosInstance } from './global';

export const fetchReviews = async (
  movieId: string | undefined,
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

export const fetchReviewByMovieAndUser = async (
  movieId: string
): Promise<Review> => {
  const review: Review = await axiosInstance
    .get(`/api/reviews?MovieId=${movieId}`)
    .then((response) => response.data);

  return review;
};

export const createReview = async (body: CreateReviewRequest) => {
  body.accountId = null;
  const response = await axiosInstance
    .post('/api/reviews', body)
    .then((response) => response.data);

  return response;
};

export const updateReview = async (body: UpdateReviewRequest) => {
  const response = await axiosInstance
    .put(`/api/reviews/${body.id}`, body)
    .then((response) => response.data);

  return response;
};

export const deleteReview = async (id: string) => {
  const response = await axiosInstance
    .delete(`/api/reviews/${id}`)
    .then((response) => response.data);

  return response;
};
