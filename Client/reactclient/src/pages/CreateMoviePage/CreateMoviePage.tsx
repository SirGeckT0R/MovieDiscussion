import { Box, Button, Grid2, Stack, TextField } from '@mui/material';
import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { CreateMovieRequest, CrewMember } from '../../types/movie';
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
import { EditCrewMembers } from '../UpdateMoviePage/components/EditCrewMembers';

export function CreateMoviePage() {
  const navigate = useNavigate();
  const { data: genres } = useQuery<Genre[]>(getGenresQuery());

  const [image, setImage] = useState<Blob | null>(null);
  const [crew, setCrew] = useState<CrewMember[]>([]);
  const {
    register: create,
    handleSubmit: handleCreate,
    control: controlCreate,
  } = useForm<CreateMovieRequest>();

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
        <ImageInput
          imageState={{ image, setImage }}
          existingImagePath={undefined}
        />
        <form onSubmit={handleCreate(onSubmit)}>
          <Grid2 container direction={'column'} gap={'20px'}>
            <TextField
              type='text'
              id='titleInput'
              label='Title'
              {...create('title')}
              required
            />
            <TextField
              type='text'
              id='descriptionInput'
              label='Description'
              {...create('description')}
              required
            />
            <Controller
              control={controlCreate}
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
              control={controlCreate}
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
        <EditCrewMembers crew={crew} setCrew={setCrew} />
      </Box>
    </Stack>
  );
}
