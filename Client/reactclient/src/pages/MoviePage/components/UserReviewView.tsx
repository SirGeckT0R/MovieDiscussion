import { Grid2, Stack, Button } from '@mui/material';
import { ReviewInput } from '../../../components/Inputs/ReviewInput';
import { DeleteReviewAction } from './DeleteReviewAction';
import { UpdateReviewForm } from './UpdateReviewForm';
import { useState } from 'react';
import { Review } from '../../../types/review';
import { ReviewCard } from '../../../components/ReviewCard/ReviewCard';

type Props = {
  movieId: string | undefined;
  userReview: Review | undefined;
  queryInvalidator: () => void;
};

export function UserReviewView({
  movieId,
  userReview,
  queryInvalidator,
}: Props) {
  const [isEditReviewMode, setIsEditReviewMode] = useState(false);

  return (
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
            onClick={() => setIsEditReviewMode(false)}>
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
  );
}
