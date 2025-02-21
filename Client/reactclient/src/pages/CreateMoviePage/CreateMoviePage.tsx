import {
  Box,
  Button,
  Grid2,
  Stack,
  TextField,
  Typography,
} from '@mui/material';
import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { CreateMovieRequest, CrewMember, CrewRole } from '../../types/movie';
import { Controller, useForm } from 'react-hook-form';
import { createMovie } from '../../api/movieService';
import { useState } from 'react';
import dayjs from 'dayjs';
import { DateTimePicker } from '@mui/x-date-pickers';
import { getGenresQuery } from '../../queries/genresQueries';
import { Genre } from '../../types/genre';
import { CrewMemberInputForm } from './components/CrewMemberInputForm';
import { ImageInput } from '../../components/Inputs/ImageInput';
import { SelectInput } from '../../components/Inputs/MultipleSelectInput';

export function CreateMoviePage() {
  const navigate = useNavigate();

  const { data: genres } = useQuery<Genre[]>(getGenresQuery());
  const [image, setImage] = useState<Blob | null>(null);
  const [crew, setCrew] = useState<CrewMember[]>([]);
  const { register, handleSubmit, control } = useForm<CreateMovieRequest>({
    defaultValues: {},
  });

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateMovieRequest) =>
      createMovie(values, image, crew),
  });

  const onSubmit = (formBody: CreateMovieRequest) => {
    mutateAsync(formBody).then(() => navigate('/movies'));
  };

  return (
    <Stack direction={'row'}>
      <Stack style={{ width: 500 }} spacing={2}>
        <ImageInput imageState={{ image, setImage }} />
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
            <SelectInput
              isMultiple={true}
              options={genres}
              control={control}
              inputName='genres'
            />
            <Button type='submit' variant='contained'>
              Create
            </Button>
          </Grid2>
        </form>
        <CrewMemberInputForm crewState={{ crew, setCrew }} />
      </Stack>
      <Box style={{ width: 400 }}>
        {crew?.map((member) => (
          <Typography color='textPrimary' key={member.personId}>
            {member.personId + ' is ' + CrewRole[member.role]}
          </Typography>
        ))}
      </Box>
    </Stack>
  );
}
