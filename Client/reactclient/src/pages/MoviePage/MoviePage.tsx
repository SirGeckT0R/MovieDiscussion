import { Button, Chip, Grid2, Typography } from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { NavLink, useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { CrewMembersView } from './components/CrewMembersView';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';

export function MoviePage() {
  const { id } = useParams();
  const { data: movie } = useQuery(getMovieQuery(id!));

  const { role } = useAuth();

  return (
    <Grid2 container gap={2} justifyContent={'center'}>
      <Stack direction={'row'} spacing={2}>
        {movie?.image ? (
          <Box
            sx={{ width: 600 }}
            component='img'
            src={`${import.meta.env.VITE_IMAGES_HOST}/${movie.image}`}
            alt={`${movie.title} - cover`}
            title={`${movie.title} - cover`}
          />
        ) : null}
        <Stack alignItems={'start'} color='info'>
          <Typography variant='h2' align='left' color='info'>
            {movie?.title}
          </Typography>
          <Typography variant='h3' align='left' color='info'>
            {movie?.description}
          </Typography>
          <Typography variant='h3'>{movie?.releaseDate}</Typography>
          <Typography variant='h4' color='info'>
            Genres:
          </Typography>
          <Stack direction={'row'} spacing={1}>
            {movie?.genres?.map((genre) => (
              <Chip label={genre.name} color='error' key={genre.id} />
            ))}
          </Stack>
          <Typography variant='h4' color='info'>
            Crew:
          </Typography>
          <CrewMembersView crew={movie?.crewMembers} />
        </Stack>
      </Stack>
      {role == Role.Admin ? (
        <NavLink to='edit'>
          <Button color='primary' variant='contained'>
            Edit
          </Button>
        </NavLink>
      ) : (
        <></>
      )}
    </Grid2>
  );
}
