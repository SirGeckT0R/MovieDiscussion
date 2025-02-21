import { Card, Divider } from '@mui/material';
import { Movie } from '../../types/movie';
import { MovieCardInfo } from './MovieCardInfo';
import { MovieCardActions } from './MovieCardActions';
import { useMovieCardStyles } from './styles/useMovieCardStyles';

export function MovieCard({ movie }: { movie: Movie }) {
  const classes = useMovieCardStyles();
  return (
    <Card className={classes.card} variant='outlined' sx={{ maxWidth: 360 }}>
      <MovieCardInfo movie={movie} />
      <Divider />
      <MovieCardActions movie={movie} />
    </Card>
  );
}
