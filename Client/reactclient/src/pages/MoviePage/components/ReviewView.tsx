import { Stack, Pagination, Grid2 } from '@mui/material';
import { ReviewList } from '../../../components/ReviewList/ReviewList';
import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import {
  getReviewsQuery,
  getUserReviewForMovieQuery,
} from '../../../queries/reviewsQueries';
import { queryClient } from '../../../api/global';
import { useAuth } from '../../../hooks/useAuth';
import { Role } from '../../../types/user';
import { UserReviewView } from './UserReviewView';

export function ReviewView({ movieId }: { movieId: string | undefined }) {
  const { user } = useAuth();
  const [pageIndex, setPageIndex] = useState(1);

  const reviewsQuery = getReviewsQuery(movieId, pageIndex);
  const { data: reviews } = useQuery(reviewsQuery);

  const userReviewQuery = getUserReviewForMovieQuery(movieId);
  const { data: userReview } = useQuery(userReviewQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(userReviewQuery.queryKey);
    queryClient.invalidateQueries(reviewsQuery.queryKey);
  };

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return (
    <Grid2 container direction={'column'} alignItems={'center'} spacing={2}>
      {user.role !== Role.Guest && (
        <UserReviewView
          userReview={userReview}
          queryInvalidator={queryInvalidator}
          movieId={movieId}
        />
      )}

      <Grid2>
        <Stack spacing={2}>
          <ReviewList reviews={reviews?.items} />
          <Pagination count={reviews?.totalPages} onChange={handlePageClick} />
        </Stack>
      </Grid2>
    </Grid2>
  );
}
