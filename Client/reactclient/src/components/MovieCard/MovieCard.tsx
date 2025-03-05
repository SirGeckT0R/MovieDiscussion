import { Box, Card, Stack } from '@mui/material';
import { Movie } from '../../types/movie';
import { MovieCardInfo } from './MovieCardInfo';
import { MovieCardActions } from './MovieCardActions';
import { useMovieCardStyles } from './styles/useMovieCardStyles';
import { NavLink } from 'react-router-dom';

export function MovieCard({
  movie,
  isInWatchlist = false,
}: {
  movie: Movie;
  isInWatchlist: boolean | undefined;
}) {
  const classes = useMovieCardStyles();

  return (
    <Card
      variant='outlined'
      sx={{ width: 240, height: 420 }}
      className={classes.card}>
      <Stack direction={'column'} spacing={2}>
        <Box height={350}>
          <NavLink to={`/movies/${movie.id}`} key={movie.id}>
            <MovieCardInfo movie={movie} />
          </NavLink>
        </Box>
        <MovieCardActions movie={movie} isInWatchlist={isInWatchlist} />
      </Stack>
    </Card>
  );
}
