export interface Review {
  id: string;
  movieId: string;
  profileId: string;
  value: string;
  text: string;
}

export interface CreateReviewRequest {
  accountId: string | null;
  movieId: string;
  value: number;
  text: string;
}

export interface PaginatedReview {
  items: Review[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
