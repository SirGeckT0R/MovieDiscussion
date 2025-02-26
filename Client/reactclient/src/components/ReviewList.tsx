import { Grid2, Typography } from '@mui/material';
import { Review } from '../types/review';

export function ReviewList({ reviews }: { reviews: Review[] }) {
  return (
    <Grid2>
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
