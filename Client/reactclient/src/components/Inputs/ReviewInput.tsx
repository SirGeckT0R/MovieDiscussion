import { Button, TextField } from '@mui/material';
import { useForm } from 'react-hook-form';
import { CreateReviewRequest } from '../../types/review';
import { useMutation } from '@tanstack/react-query';
import { createReview } from '../../api/reviewService';

export function ReviewInput({ movieId }: { movieId: string }) {
  const { register, handleSubmit } = useForm<CreateReviewRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateReviewRequest) => createReview(values),
  });

  const onSubmit = (values: CreateReviewRequest) => {
    values.movieId = movieId;
    mutateAsync(values);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <TextField
        {...register('value')}
        fullWidth
        name='value'
        id='value'
        type='number'
        label='Rating'
        required
        slotProps={{
          input: { inputProps: { min: 1, max: 10 } },
        }}
      />
      <TextField
        {...register('text')}
        fullWidth
        name='text'
        id='text'
        type='text'
        label='Text'
        required
        multiline
        rows={6}
      />
      <Button type='submit'>Add review</Button>
    </form>
  );
}
