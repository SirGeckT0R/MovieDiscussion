import { Grid2 } from '@mui/material';
import { Movie } from '../types/movie';
import { MovieCard } from './MovieCard/MovieCard';

export function MovieList({
  movies,
  watchlist,
}: {
  movies: Movie[] | undefined;
  watchlist: string[] | undefined;
}) {
  return (
    <Grid2 container spacing={2} columns={3}>
      {movies?.map((movie) => (
        <MovieCard
          key={movie.id}
          movie={movie}
          isInWatchlist={watchlist?.includes(movie.id)}
        />
      ))}
    </Grid2>
  );
}
