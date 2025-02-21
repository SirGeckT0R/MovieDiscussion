import { Button, Grid2, Stack, TextField, Typography } from '@mui/material';
import { CreateGenreRequest, UpdateGenreRequest } from '../../types/genre';
import { useForm } from 'react-hook-form';
import { useMutation, useQuery } from '@tanstack/react-query';
import { createGenre, updateGenre } from '../../api/genreService';
import { SelectInput } from '../../components/Inputs/MultipleSelectInput';
import { getGenresQuery } from '../../queries/genresQueries';
import { useNavigate } from 'react-router-dom';

export function ModifyGenresPage() {
  const navigate = useNavigate();

  const { data: genres } = useQuery(getGenresQuery());
  const { register, handleSubmit } = useForm<CreateGenreRequest>();
  const {
    register: update,
    control: updateControl,
    handleSubmit: handleUpdate,
  } = useForm<UpdateGenreRequest>();

  const { mutateAsync: createMutation } = useMutation({
    mutationFn: (values: CreateGenreRequest) => createGenre(values),
  });
  const { mutateAsync: updateMutation } = useMutation({
    mutationFn: (values: UpdateGenreRequest) => updateGenre(values),
  });

  const onCreateSubmit = (formBody: CreateGenreRequest) => {
    createMutation(formBody).then(() => navigate(0));
  };

  const onUpdateSubmit = (formBody: UpdateGenreRequest) => {
    updateMutation(formBody).then(() => navigate(0));
  };

  return (
    <Stack style={{ width: 500 }} spacing={6}>
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

      <form onSubmit={handleUpdate(onUpdateSubmit)}>
        <Stack spacing={2}>
          <Typography variant='h5' color='textPrimary'>
            Modify existing ones
          </Typography>
          <SelectInput
            isMultiple={false}
            options={genres}
            control={updateControl}
            inputName='id'
          />
          <TextField
            type='text'
            id='nameInput'
            label='New Name'
            {...update('name')}
            required
          />
          <Button type='submit' variant='contained'>
            Update
          </Button>
        </Stack>
      </form>
    </Stack>
  );
}
