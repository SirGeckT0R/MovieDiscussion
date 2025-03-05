import { Paper, Stack, Rating, Typography } from '@mui/material';
import { Review } from '../types/review';

export function ReviewCard({ review }: { review: Review }) {
  return (
    <Paper sx={{ mb: 2, borderRadius: 4 }}>
      <Stack padding={2}>
        <Rating value={review.value / 2} max={5} precision={0.5} readOnly />
        <Typography
          variant='h5'
          color='info'
          key={review.id}
          textAlign={'left'}>
          {review.text}
        </Typography>
      </Stack>
    </Paper>
  );
}
