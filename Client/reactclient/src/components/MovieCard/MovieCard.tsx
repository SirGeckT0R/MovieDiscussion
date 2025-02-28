import { Card, Stack } from '@mui/material';
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
      sx={{ width: 360, height: 600 }}
      className={classes.card}>
      <Stack direction={'column'} spacing={2}>
        <NavLink to={`/movies/${movie.id}`} key={movie.id}>
          <MovieCardInfo movie={movie} />
        </NavLink>
        <MovieCardActions movie={movie} isInWatchlist={isInWatchlist} />
      </Stack>
    </Card>
  );
}
