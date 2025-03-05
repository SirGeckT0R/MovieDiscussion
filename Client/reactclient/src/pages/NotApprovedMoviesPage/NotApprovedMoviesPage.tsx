import { useQuery } from '@tanstack/react-query';
import { getNotApprovedMoviesQuery } from '../../queries/moviesQueries';
import { queryClient } from '../../api/global';
import { Box, Card, Stack, Typography } from '@mui/material';
import { useMovieCardStyles } from '../../components/MovieCard/styles/useMovieCardStyles';
import { CrewMembersView } from '../../components/CrewMembersView';
import { DateDisplay } from '../../components/DateDisplay';
import { MovieGenresView } from '../../components/MovieGenresView';
import { MovieApprovalActions } from './components/MovieApprovalActions';

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
              <Typography variant='h3' align='left' color='info'>
                {movie?.description}
              </Typography>
              <DateDisplay date={new Date(movie?.releaseDate ?? '')} />
              <Typography variant='h4' color='info'>
                Genres:
              </Typography>
              <MovieGenresView genres={movie.genres} />
              <Typography variant='h4' color='info'>
                Crew:
              </Typography>
              <CrewMembersView crew={movie?.crewMembers} />
            </Stack>
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
