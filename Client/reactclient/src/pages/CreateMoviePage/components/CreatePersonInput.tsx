import { Backdrop, Button, Stack, TextField } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import { useMutation } from '@tanstack/react-query';
import dayjs from 'dayjs';
import { Dispatch, SetStateAction } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { createPerson } from '../../../api/peopleService';
import { CreatePersonRequest } from '../../../types/people';

export function CreatePersonInput({
  openState,
}: {
  openState: { open: boolean; setOpen: Dispatch<SetStateAction<boolean>> };
}) {
  const {
    register: person,
    handleSubmit: handlePersonSubmit,
    control: personControl,
  } = useForm<CreatePersonRequest>();

  const { mutateAsync: mutatePersonAsync } = useMutation({
    mutationFn: (values: CreatePersonRequest) => createPerson(values),
  });

  const onPersonSubmit = (formBody: CreatePersonRequest) => {
    mutatePersonAsync(formBody);
  };

  return (
    <Backdrop
      sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
      open={openState.open}>
      <form onSubmit={handlePersonSubmit(onPersonSubmit)}>
        <Button variant='contained' onClick={() => openState.setOpen(false)}>
          Exit
        </Button>
        <Stack spacing={2}>
          <TextField
            type='text'
            id='firstNameInput'
            label='First Name'
            {...person('firstName')}
            required
          />
          <TextField
            type='text'
            id='lastNameInput'
            label='LastName'
            {...person('lastName')}
            required
          />
          <Controller
            control={personControl}
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
          <Button type='submit'>Submit</Button>
        </Stack>
      </form>
    </Backdrop>
  );
}
