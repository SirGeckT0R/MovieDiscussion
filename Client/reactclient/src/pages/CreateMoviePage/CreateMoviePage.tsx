import { Box, Stack } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { CreateMovieRequest, CrewMember } from '../../types/movie';
import { createMovie } from '../../api/movieService';
import { useState } from 'react';
import { CrewMemberInputForm } from './components/CrewMemberInputForm';
import { ImageInput } from '../../components/Inputs/ImageInput';
import { EditCrewMembers } from '../UpdateMoviePage/components/EditCrewMembers';
import { MovieInfoForm } from './components/MovieInfoForm';

export function CreateMoviePage() {
  const navigate = useNavigate();

  const [image, setImage] = useState<Blob | null>(null);
  const [crew, setCrew] = useState<CrewMember[]>([]);

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
        <MovieInfoForm onSubmit={onSubmit} />
        <CrewMemberInputForm crewState={{ crew, setCrew }} />
      </Stack>
      <Box style={{ width: 400 }}>
        <EditCrewMembers crew={crew} setCrew={setCrew} />
      </Box>
    </Stack>
  );
}
