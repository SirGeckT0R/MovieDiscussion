import { Grid2 } from '@mui/material';
import { MovieCard } from '../MovieCard/MovieCard';
import { useAuth } from '../../hooks/useAuth';
import { Movie } from '../../types/movie';
import { Role } from '../../types/user';
import { getWatchlistQuery } from '../../queries/watchlistsQueries';
import { useQuery } from '@tanstack/react-query';

export function MovieList({ movies }: { movies: Movie[] | undefined }) {
  const { user } = useAuth();
  const isAuthenticated = user.role === Role.User;

  const watchlistQuery = getWatchlistQuery(isAuthenticated);
  const { data: watchlist } = useQuery(watchlistQuery);

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
