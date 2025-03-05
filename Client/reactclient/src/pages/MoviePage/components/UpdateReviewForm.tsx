import { TextField, Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { Review, UpdateReviewRequest } from '../../../types/review';
import { updateReview } from '../../../api/reviewService';
import { useForm } from 'react-hook-form';
import { Dispatch, SetStateAction } from 'react';

export function UpdateReviewForm({
  userReview,
  queryInvalidator,
  editMode,
}: {
  userReview: Review | undefined;
  queryInvalidator: () => void;
  editMode: Dispatch<SetStateAction<boolean>>;
}) {
  const { register, handleSubmit } = useForm<UpdateReviewRequest>({
    defaultValues: {
      id: userReview?.id,
      text: userReview?.text,
      value: userReview?.value,
    },
  });

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdateReviewRequest) => updateReview(values),
  });

  const onReviewUpdate = (values: UpdateReviewRequest) => {
    values.id = userReview?.id ?? '';
    mutateAsync(values)
      .then(queryInvalidator)
      .then(() => editMode(false));
  };

  return (
    <div>
      <form onClick={handleSubmit(onReviewUpdate)}>
        <TextField
          {...register('value')}
          fullWidth
          name='value'
          id='value'
          type='number'
          label='Rating'
          required
          defaultValue={userReview?.value}
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
          defaultValue={userReview?.text}
          required
          multiline
          rows={6}
        />
        <Button type='submit' variant='contained'>
          Update
        </Button>
      </form>
    </div>
  );
}
