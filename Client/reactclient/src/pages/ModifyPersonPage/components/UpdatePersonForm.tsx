import { Button, Stack, TextField, Typography } from '@mui/material';
import { UpdatePersonRequest } from '../../../types/people';
import { Controller, useForm } from 'react-hook-form';
import { useMutation } from '@tanstack/react-query';
import { fetchPeople, updatePerson } from '../../../api/peopleService';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

export function UpdatePersonForm() {
  const { register, handleSubmit, control, reset } =
    useForm<UpdatePersonRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdatePersonRequest) => updatePerson(values),
  });

  const onUpdateSubmit = (values: UpdatePersonRequest) => {
    mutateAsync(values).then(reset);
  };

  return (
    <form onSubmit={handleSubmit(onUpdateSubmit)}>
      <Stack spacing={2}>
        <Typography variant='h5'>Modify existing ones</Typography>
        <DebounceSearch
          inputName='id'
          control={control}
          searchData={{ key: 'people', searchFetch: fetchPeople }}
        />
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
          Update
        </Button>
      </Stack>
    </form>
  );
}
