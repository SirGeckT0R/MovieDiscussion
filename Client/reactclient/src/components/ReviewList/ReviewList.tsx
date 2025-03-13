import { Grid2, Typography } from '@mui/material';
import { Review } from '../../types/review';
import { ReviewCard } from '../ReviewCard/ReviewCard';

export function ReviewList({ reviews }: { reviews: Review[] | undefined }) {
  return (
    <Grid2>
      <Typography variant='h4'>Reviews for this movie</Typography>
      {reviews?.map((review) => (
        <ReviewCard review={review} key={review.id} />
      ))}
    </Grid2>
  );
}
