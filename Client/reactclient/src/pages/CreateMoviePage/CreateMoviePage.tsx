import {
  Backdrop,
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
import { CreatePersonRequest } from '../../types/people';
import { createPerson } from '../../api/peopleService';

export function CreateMoviePage() {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);

  const { data: genres } = useQuery<Genre[]>(getGenresQuery());
  const [image, setImage] = useState<Blob | null>(null);
  const [crew, setCrew] = useState<CrewMember[]>([]);
  const { register, handleSubmit, control } = useForm<CreateMovieRequest>({
    defaultValues: {},
  });

  const {
    register: person,
    handleSubmit: handlePersonSubmit,
    control: personControl,
  } = useForm<CreatePersonRequest>({
    defaultValues: {},
  });

  const { mutateAsync } = useMutation({
    mutationFn: (values: CreateMovieRequest) =>
      createMovie(values, image, crew),
  });

  const { mutateAsync: mutatePersonAsync } = useMutation({
    mutationFn: (values: CreatePersonRequest) => createPerson(values),
  });

  const onSubmit = (formBody: CreateMovieRequest) => {
    mutateAsync(formBody).then(() => navigate('/movies'));
  };

  const onPersonSubmit = (formBody: CreatePersonRequest) => {
    console.log(formBody);
    mutatePersonAsync(formBody);
  };

  return (
    <Stack direction={'row'}>
      <Stack style={{ width: 500 }} spacing={2}>
        <ImageInput
          imageState={{ image, setImage }}
          existingImagePath={undefined}
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
              Create
            </Button>
          </Grid2>
        </form>
        <CrewMemberInputForm crewState={{ crew, setCrew }} />
      </Stack>
      <Button color='primary' variant='contained' onClick={() => setOpen(true)}>
        Create Person
      </Button>
      <Backdrop
        sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
        open={open}>
        <form onSubmit={handlePersonSubmit(onPersonSubmit)}>
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
        </form>
      </Backdrop>
      <Box style={{ width: 400 }}>
        {crew?.map((member) => (
          <Typography key={member.personId}>
            {member.fullName + ' is ' + CrewRole[member.role]}
          </Typography>
        ))}
      </Box>
    </Stack>
  );
}
