import { Typography, TextField, Button, Stack } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import { useMutation } from '@tanstack/react-query';
import dayjs from 'dayjs';
import { Controller, useForm } from 'react-hook-form';
import { createDiscussion } from '../../../api/discussionsService';
import { CreateDiscussionRequest } from '../../../types/discussion';
import { Dispatch, SetStateAction } from 'react';

export function CreateDiscussionForm({
  queryInvalidator,
  createMode,
}: {
  queryInvalidator: () => void;
  createMode: Dispatch<SetStateAction<boolean>>;
}) {
  const { register, handleSubmit, control } =
    useForm<CreateDiscussionRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateDiscussionRequest) => createDiscussion(values),
  });

  const onCreateDiscussion = (values: CreateDiscussionRequest) => {
    mutateAsync(values)
      .then(queryInvalidator)
      .then(() => createMode(false));
  };

  return (
    <form onSubmit={handleSubmit(onCreateDiscussion)}>
      <Stack spacing={2} sx={{ width: 600, margin: '0 auto' }}>
        <Typography variant='h3'>Create a discussion</Typography>
        <TextField
          type='text'
          id='titleInput'
          label='Title'
          {...register('title')}
          required
        />
        <TextField
          type='text'
          id='descriptionInput'
          label='Description'
          {...register('description')}
          required
        />
        <Controller
          control={control}
          name='startAt'
          rules={{ required: true }}
          render={({ field: { value, ...props } }) => (
            <DateTimePicker
              label='Start Date *'
              value={value ? dayjs(value) : null}
              {...props}
              sx={{ width: '100%' }}
            />
          )}
        />
        <Button type='submit' variant='contained'>
          Create
        </Button>
      </Stack>
    </form>
  );
}
