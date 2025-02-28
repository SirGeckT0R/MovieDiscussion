import { Button, Chip, Typography } from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { NavLink, useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { CrewMembersView } from './components/CrewMembersView';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';
import { DateDisplay } from './components/DateDisplay';
import { deleteMovie } from '../../api/movieService';
import { ReviewView } from './components/ReviewView';

export function MoviePage() {
  const { role } = useAuth();

  const { id } = useParams();
  const { data: movie } = useQuery(getMovieQuery(id!));

  const handleDeleteMovie = () => {
    deleteMovie(movie?.id ?? '', movie?.image ?? '');
  };

  return (
    <Stack spacing={2} direction={'column'} justifyContent={'center'}>
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
        <Stack alignItems={'start'} color='info' spacing={2}>
          <Typography variant='h2' align='left' color='info'>
            {movie?.title}
          </Typography>
          {role == Role.Admin ? (
            <Stack direction={'row'} spacing={2}>
              <NavLink to='edit'>
                <Button color='primary' variant='contained'>
                  Edit
                </Button>
              </NavLink>
              <Button
                color='error'
                variant='contained'
                onClick={handleDeleteMovie}>
                Delete
              </Button>
            </Stack>
          ) : (
            <></>
          )}
          <Typography variant='h3' align='left' color='info'>
            {movie?.description}
          </Typography>
          <DateDisplay date={new Date(movie?.releaseDate ?? '')} />
          <Typography variant='h4' color='info'>
            Genres:
          </Typography>
          <Stack direction={'row'} spacing={1}>
            {movie?.genres?.map((genre) => (
              <Chip
                label={genre.name}
                color='primary'
                sx={{
                  fontWeight: 'bold',
                }}
                key={genre.id}
              />
            ))}
          </Stack>
          <Typography variant='h4' color='info'>
            Crew:
          </Typography>
          <CrewMembersView crew={movie?.crewMembers} />
        </Stack>
      </Stack>
      <ReviewView movieId={id} />
    </Stack>
  );
}
