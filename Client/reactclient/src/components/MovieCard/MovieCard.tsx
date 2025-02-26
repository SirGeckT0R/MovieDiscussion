import { Card, Divider } from '@mui/material';
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
      className={classes.card}
      variant='outlined'
      sx={{ width: 360, height: 360 }}>
      <NavLink to={`/movies/${movie.id}`} key={movie.id}>
        <MovieCardInfo movie={movie} />
      </NavLink>
      <Divider />
      <MovieCardActions movie={movie} isInWatchlist={isInWatchlist} />
    </Card>
  );
}
