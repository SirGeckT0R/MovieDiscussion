import { Typography } from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { CrewMembersView } from '../../components/CrewMembersView';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';
import { DateDisplay } from '../../components/DateDisplay';
import { ReviewView } from './components/ReviewView';
import { MovieAdminActions } from './components/MovieAdminActions';
import { MovieGenresView } from '../../components/MovieGenresView';

export function MoviePage() {
  const { user } = useAuth();

  const { id } = useParams();
  const { data: movie } = useQuery(getMovieQuery(id!));

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
          {user.role == Role.Admin ? (
            <MovieAdminActions id={id} image={movie?.image} />
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
          <MovieGenresView genres={movie?.genres} />
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
