import {
  Button,
  Typography,
  Stack,
  Pagination,
  Grid2,
  Box,
} from '@mui/material';
import { ReviewInput } from '../../../components/Inputs/ReviewInput';
import { ReviewList } from '../../../components/ReviewList';
import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import {
  getReviewsQuery,
  getUserReviewForMovieQuery,
} from '../../../queries/reviewsQueries';
import { queryClient } from '../../../api/global';
import { UpdateReviewForm } from './UpdateReviewForm';
import { DeleteReviewAction } from './DeleteReviewAction';
import { ReviewCard } from '../../../components/ReviewCard';

export function ReviewView({ movieId }: { movieId: string | undefined }) {
  const [pageIndex, setPageIndex] = useState(1);
  const [isEditReviewMode, setIsEditReviewMode] = useState(false);

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
      <Grid2>
        {isEditReviewMode ? (
          <Stack spacing={2}>
            <UpdateReviewForm
              userReview={userReview}
              queryInvalidator={queryInvalidator}
              editMode={setIsEditReviewMode}
            />
            <Button
              color='warning'
              variant='contained'
              onClick={() => {
                console.log('here');
                setIsEditReviewMode(false);
              }}>
              Cancel Edit
            </Button>
          </Stack>
        ) : userReview ? (
          <Grid2
            container
            direction={'column'}
            alignItems={'start'}
            color='info'
            spacing={2}
            size='grow'>
            <ReviewCard review={userReview} />
            <Stack direction={'row'} spacing={2}>
              <DeleteReviewAction
                queryInvalidator={queryInvalidator}
                id={userReview?.id}
              />
              <Button
                color='warning'
                variant='contained'
                onClick={() => setIsEditReviewMode(true)}>
                Edit
              </Button>
            </Stack>
          </Grid2>
        ) : (
          <ReviewInput movieId={movieId} queryInvalidator={queryInvalidator} />
        )}
      </Grid2>
      <Grid2>
        <Stack spacing={2}>
          <ReviewList reviews={reviews?.items} />
          <Pagination count={reviews?.totalPages} onChange={handlePageClick} />
        </Stack>
      </Grid2>
    </Grid2>
  );
}
