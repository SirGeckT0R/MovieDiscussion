import { Button, Typography, Stack, Pagination } from '@mui/material';
import { ReviewInput } from '../../../components/Inputs/ReviewInput';
import { ReviewList } from '../../../components/ReviewList';
import { useMutation, useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { deleteReview } from '../../../api/reviewService';
import {
  getReviewsQuery,
  getUserReviewForMovieQuery,
} from '../../../queries/reviewsQueries';
import { queryClient } from '../../../api/global';
import { UpdateReviewForm } from './UpdateReviewForm';

export function ReviewView({ movieId }: { movieId: string | undefined }) {
  const [pageIndex, setPageIndex] = useState(1);
  const [isEditReviewMode, setIsEditReviewMode] = useState(false);

  const reviewsQuery = getReviewsQuery(movieId, pageIndex);
  const { data: reviews } = useQuery(reviewsQuery);

  const userReviewQuery = getUserReviewForMovieQuery(movieId);
  const { data: userReview } = useQuery(userReviewQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries({
      queryKey: userReviewQuery.queryKey,
    });
    queryClient.invalidateQueries({ queryKey: reviewsQuery.queryKey });
  };

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  const { mutateAsync: deleteReviewAsync } = useMutation({
    mutationFn: () => deleteReview(userReview?.id ?? ''),
  });

  const handleDeleteReview = () => {
    deleteReviewAsync().then(queryInvalidator);
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
          <Button
            color='error'
            variant='contained'
            onClick={handleDeleteReview}>
            Delete
          </Button>
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
