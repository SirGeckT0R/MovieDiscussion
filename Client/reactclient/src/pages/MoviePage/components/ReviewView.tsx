import { Button, Typography, Stack, Pagination } from '@mui/material';
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
    <>
      {isEditReviewMode ? (
        <Stack spacing={2} alignItems={'center'}>
          <UpdateReviewForm
            userReview={userReview}
            queryInvalidator={queryInvalidator}
            editMode={setIsEditReviewMode}
          />
          <Button
            color='warning'
            variant='contained'
            onClick={() => setIsEditReviewMode(false)}>
            Cancel Edit
          </Button>
        </Stack>
      ) : userReview ? (
        <Stack direction={'row'} spacing={2} justifyContent={'center'}>
          <Typography variant='h3'>
            Your review: {userReview?.text + ' ' + userReview?.value}
          </Typography>
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
      ) : (
        <ReviewInput movieId={movieId} queryInvalidator={queryInvalidator} />
      )}

      <Stack spacing={2} direction={'column'} alignItems={'center'}>
        <ReviewList reviews={reviews?.items} />
        <Pagination count={reviews?.totalPages} onChange={handlePageClick} />
      </Stack>
    </>
  );
}
