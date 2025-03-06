import { Grid2, TextField, Button } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import { Controller, useForm } from 'react-hook-form';
import { CustomSelectInput } from '../../../components/Inputs/MultipleSelectInput';
import { useQuery } from '@tanstack/react-query';
import { getGenresQuery } from '../../../queries/genresQueries';
import { Genre } from '../../../types/genre';
import { CreateMovieRequest } from '../../../types/movie';

type Props = {
  onSubmit: (values: CreateMovieRequest) => void;
};

export function MovieInfoForm({ onSubmit }: Props) {
  const genreQuery = getGenresQuery();
  const { data: genres } = useQuery<Genre[]>(genreQuery);

  const { register, handleSubmit, control } = useForm<CreateMovieRequest>();

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Grid2 container direction={'column'} gap={'20px'}>
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
          name='releaseDate'
          rules={{ required: true }}
          render={({ field: { value, ...props } }) => (
            <DateTimePicker
              label='Release Date *'
              value={value ? dayjs(value) : null}
              {...props}
              sx={{ width: '100%' }}
            />
          )}
        />
        <CustomSelectInput
          isMultiple={true}
          label='Genres'
          options={genres}
          control={control}
          inputName='genres'
        />
        <Button type='submit' variant='contained'>
          Create
        </Button>
      </Grid2>
    </form>
  );
}
