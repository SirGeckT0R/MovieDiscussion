import { useQuery } from '@tanstack/react-query';
import { getNotApprovedMoviesQuery } from '../../queries/moviesQueries';
import { queryClient } from '../../api/global';
import { Box, Card, Grid2, Stack, Typography } from '@mui/material';
import { useMovieCardStyles } from '../../components/MovieCard/styles/useMovieCardStyles';
import { MovieApprovalActions } from './components/MovieApprovalActions';
import { ImageSharp } from '@mui/icons-material';
import { emptyImageStyle } from '../../components/MovieCard/styles/emptyImageStyle';
import { DateWithListsView } from '../../components/MovieFieldsView/DateWithListsView';
import { GeneralInfoView } from '../../components/MovieFieldsView/GeneralInfoView';

export function NotApprovedMoviesPage() {
  const classes = useMovieCardStyles();

  const notApprovedMoviesQuery = getNotApprovedMoviesQuery();
  const { data: movies } = useQuery(notApprovedMoviesQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(notApprovedMoviesQuery.queryKey);
  };

  return (
    <Stack spacing={5}>
      {movies?.map((movie) => (
        <Card
          className={classes.card}
          variant='outlined'
          sx={{ width: '100%' }}
          key={movie.id}>
          <Stack spacing={2} direction={'column'} justifyContent={'center'}>
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
                  <GeneralInfoView movie={movie} allowActions={false} />
                  <Stack direction={'row'} spacing={20} width={'100%'}>
                    <DateWithListsView movie={movie} />
                  </Stack>
                </Grid2>
              </Grid2>
            </Grid2>
            <MovieApprovalActions
              id={movie.id}
              queryInvalidator={queryInvalidator}
            />
          </Stack>
        </Card>
      ))}
    </Stack>
  );
}
