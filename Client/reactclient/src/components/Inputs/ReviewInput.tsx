import { Button, Stack, TextField } from '@mui/material';
import { useForm } from 'react-hook-form';
import { CreateReviewRequest } from '../../types/review';
import { useMutation } from '@tanstack/react-query';
import { createReview } from '../../api/reviewService';

export function ReviewInput({
  movieId,
  queryInvalidator,
}: {
  movieId: string | undefined;
  queryInvalidator: () => void;
}) {
  const { register, handleSubmit } = useForm<CreateReviewRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateReviewRequest) => createReview(values),
  });

  const onSubmit = (values: CreateReviewRequest) => {
    values.movieId = movieId ?? '';
    mutateAsync(values).then(queryInvalidator);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Stack spacing={2} sx={{ width: 500 }}>
        <TextField
          {...register('value')}
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
          name='text'
          id='text'
          type='text'
          label='Text'
          required
          multiline
          rows={6}
        />
        <Button type='submit' variant='contained'>
          Add review
        </Button>
      </Stack>
    </form>
  );
}
