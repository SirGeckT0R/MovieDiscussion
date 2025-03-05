import { Grid2 } from '@mui/material';
import { Movie } from '../types/movie';
import { MovieCard } from './MovieCard/MovieCard';
import { useAuth } from '../hooks/useAuth';
import { Role } from '../types/user';
import { useWatchlist } from '../hooks/useWatchlist';

export function MovieList({ movies }: { movies: Movie[] | undefined }) {
  const { user } = useAuth();
  const isAuthenticated = user.role === Role.User;

  const { data: watchlist } = useWatchlist(isAuthenticated);

  return (
    <Grid2 container spacing={2} columns={3} height={860}>
      {movies?.map((movie) => (
        <Grid2 key={movie.id} size={1}>
          <MovieCard
            movie={movie}
            isInWatchlist={watchlist?.movies
              ?.map((movie) => movie.id)
              .includes(movie.id)}
          />
        </Grid2>
      ))}
    </Grid2>
  );
}
