import { Grid2, Typography } from '@mui/material';
import { Review } from '../types/review';

export function ReviewList({ reviews }: { reviews: Review[] | undefined }) {
  return (
    <Grid2>
      <Typography variant='h4'>Reviews for this movie</Typography>
      {reviews?.map((review) => (
        <div key={review.id}>
          <Typography variant='h4' color='secondary'>
            {review.text + ' ' + review.value}
          </Typography>
        </div>
      ))}
    </Grid2>
  );
}
