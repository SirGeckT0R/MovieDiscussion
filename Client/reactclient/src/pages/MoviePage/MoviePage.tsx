import { Grid2, Rating, Typography } from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { CrewMembersView } from '../../components/MovieFieldsView/CrewMembersView';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';
import { DateDisplay } from '../../components/MovieFieldsView/DateDisplay';
import { ReviewView } from './components/ReviewView';
import { MovieAdminActions } from './components/MovieAdminActions';
import { MovieGenresView } from '../../components/MovieFieldsView/MovieGenresView';
import { ImageSharp } from '@mui/icons-material';
import { emptyImageStyle } from '../../components/MovieCard/styles/emptyImageStyle';
import { DateWithListsView } from '../../components/MovieFieldsView/DateWithListsView';

export function MoviePage() {
  const { id } = useParams();
  const { user } = useAuth();

  const movieQuery = getMovieQuery(id);
  const { data: movie } = useQuery(movieQuery);

  return (
    <Grid2 container direction={'column'} spacing={2}>
      <Grid2 container spacing={2}>
        <Grid2 size='auto'>
          {movie?.image ? (
            <Box
              width='600px'
              component='img'
              src={`${import.meta.env.VITE_IMAGES_HOST}/${movie.image}`}
              alt={`${movie.title} - cover`}
              title={`${movie.title} - cover`}
              sx={{ objectFit: 'contain' }}
            />
          ) : (
            <ImageSharp sx={emptyImageStyle} />
          )}
        </Grid2>
        <Grid2
          container
          direction={'column'}
          alignItems={'start'}
          color='info'
          spacing={2}
          size='grow'>
          <Stack spacing={2}>
            <Typography
              variant='h2'
              align='left'
              color='info'
              fontWeight={'bold'}>
              {movie?.title}
            </Typography>
            {user.role === Role.Admin && (
              <MovieAdminActions id={id} image={movie?.image} />
            )}

            <Rating
              value={movie ? movie.rating / 2 : 0}
              max={5}
              precision={0.5}
              readOnly
            />
            <Typography
              variant='h3'
              align='left'
              color='info'
              sx={{ wordBreak: 'break-word' }}>
              {movie?.description}
            </Typography>
          </Stack>
          <Stack direction={'row'} spacing={20} width={'100%'}>
            <DateWithListsView movie={movie} />
            <ReviewView movieId={id} />
          </Stack>
        </Grid2>
      </Grid2>
    </Grid2>
  );
}
