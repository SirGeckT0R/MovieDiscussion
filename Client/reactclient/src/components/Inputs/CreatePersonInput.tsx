import { Button, Stack, TextField } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import { useMutation } from '@tanstack/react-query';
import dayjs from 'dayjs';
import { Controller, useForm } from 'react-hook-form';
import { createPerson } from '../../api/peopleService';
import { CreatePersonRequest } from '../../types/people';

export function CreatePersonInput() {
  const { register, handleSubmit, control, reset } =
    useForm<CreatePersonRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreatePersonRequest) => createPerson(values),
  });

  const onPersonSubmit = (formBody: CreatePersonRequest) => {
    mutateAsync(formBody).then(reset);
  };

  return (
    <form onSubmit={handleSubmit(onPersonSubmit)}>
      <Stack spacing={2}>
        <TextField
          type='text'
          id='firstNameInput'
          label='First Name'
          {...register('firstName')}
          required
        />
        <TextField
          type='text'
          id='lastNameInput'
          label='LastName'
          {...register('lastName')}
          required
        />
        <Controller
          control={control}
          name='dateOfBirth'
          rules={{ required: true }}
          render={({ field: { value, ...props } }) => (
            <DateTimePicker
              label='Date of Birth *'
              value={value ? dayjs(value) : null}
              {...props}
              sx={{ width: '100%' }}
            />
          )}
        />
        <Button type='submit' variant='contained'>
          Add new
        </Button>
      </Stack>
    </form>
  );
}
