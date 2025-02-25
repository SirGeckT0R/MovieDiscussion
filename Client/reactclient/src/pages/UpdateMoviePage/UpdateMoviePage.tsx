import {
  Box,
  Button,
  Grid2,
  Stack,
  TextField,
  Typography,
} from '@mui/material';
import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate, useParams } from 'react-router-dom';
import {
  CrewMember,
  CrewRole,
  Movie,
  UpdateMovieRequest,
} from '../../types/movie';
import { Controller, useForm } from 'react-hook-form';
import { useState } from 'react';
import dayjs from 'dayjs';
import { DateTimePicker } from '@mui/x-date-pickers';
import { getGenresQuery } from '../../queries/genresQueries';
import { Genre } from '../../types/genre';
import { ImageInput } from '../../components/Inputs/ImageInput';
import { SelectInput } from '../../components/Inputs/MultipleSelectInput';
import { CrewMemberInputForm } from '../CreateMoviePage/components/CrewMemberInputForm';
import { getMovieQuery } from '../../queries/moviesQueries';
import { updateMovie } from '../../api/movieService';
import { CrewMembersView } from '../MoviePage/components/CrewMembersView';
import { EditCrewMembers } from './components/EditCrewMembers';

export function UpdateMoviePage() {
  const { id } = useParams();
  const { data: movie } = useQuery<Movie>(getMovieQuery(id!));
  const navigate = useNavigate();

  const { data: genres } = useQuery<Genre[]>(getGenresQuery());
  const [image, setImage] = useState<Blob | null>(null);
  const [crew, setCrew] = useState<CrewMember[]>(movie!.crewMembers);
  const { register, handleSubmit, control } = useForm<UpdateMovieRequest>({
    defaultValues: {
      id: movie?.id,
      title: movie?.title,
      description: movie?.description,
      releaseDate: movie?.releaseDate,
      genres: movie?.genres.map((x) => x.id),
      crewMembers: movie?.crewMembers,
      image: movie?.image,
    },
  });

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdateMovieRequest) =>
      updateMovie(values, image, crew),
  });

  const onSubmit = (formBody: UpdateMovieRequest) => {
    mutateAsync(formBody).then(() => navigate('/movies'));
  };

  return (
    <Stack direction={'row'}>
      <Stack style={{ width: 500 }} spacing={2}>
        <ImageInput
          imageState={{ image, setImage }}
          existingImagePath={movie?.image}
        />
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
              Update
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
