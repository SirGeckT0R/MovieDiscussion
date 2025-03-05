import { Grid2, TextField, Button } from '@mui/material';
import { CreateGenreRequest } from '../../../types/genre';
import { useMutation } from '@tanstack/react-query';
import { createGenre } from '../../../api/genreService';
import { useForm } from 'react-hook-form';

export function CreateGenreForm({
  queryInvalidator,
}: {
  queryInvalidator: () => void;
}) {
  const { register, handleSubmit } = useForm<CreateGenreRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateGenreRequest) => createGenre(values),
  });

  const onCreateSubmit = (formBody: CreateGenreRequest) => {
    mutateAsync(formBody).then(queryInvalidator);
  };

  return (
    <form onSubmit={handleSubmit(onCreateSubmit)}>
      <Grid2 container direction={'column'} gap={'20px'}>
        <TextField
          type='text'
          id='nameInput'
          label='Name'
          {...register('name')}
          required
        />
        <Button type='submit' variant='contained'>
          Add new
        </Button>
      </Grid2>
    </form>
  );
}
