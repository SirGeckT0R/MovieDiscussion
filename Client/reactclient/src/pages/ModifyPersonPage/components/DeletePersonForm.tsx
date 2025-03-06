import { Button, Stack, Typography } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { fetchPeople, deletePerson } from '../../../api/peopleService';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';
import { DeletePersonRequest } from '../../../types/people';

export function DeletePersonForm() {
  const { handleSubmit, control, reset } = useForm<DeletePersonRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: DeletePersonRequest) => deletePerson(values),
  });

  const onDeleteSubmit = (values: DeletePersonRequest) => {
    mutateAsync(values).then(reset);
  };

  return (
    <form onSubmit={handleSubmit(onDeleteSubmit)}>
      <Stack spacing={2}>
        <Typography variant='h5'>Delete</Typography>
        <DebounceSearch
          inputName='id'
          control={control}
          searchData={{ key: 'people', searchFetch: fetchPeople }}
        />
        <Button type='submit' variant='contained'>
          Delete
        </Button>
      </Stack>
    </form>
  );
}
