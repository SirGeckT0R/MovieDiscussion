import { Grid2 } from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { ReviewView } from './components/ReviewView';
import { ImageSharp } from '@mui/icons-material';
import { emptyImageStyle } from '../../components/MovieCard/styles/emptyImageStyle';
import { DateWithListsView } from '../../components/MovieFieldsView/DateWithListsView';
import { GeneralInfoView } from '../../components/MovieFieldsView/GeneralInfoView';

export function MoviePage() {
  const { id } = useParams();

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
          <GeneralInfoView movie={movie} />
          <Stack direction={'row'} spacing={20} width={'100%'}>
            <DateWithListsView movie={movie} />
            <ReviewView movieId={id} />
          </Stack>
        </Grid2>
      </Grid2>
    </Grid2>
  );
}
