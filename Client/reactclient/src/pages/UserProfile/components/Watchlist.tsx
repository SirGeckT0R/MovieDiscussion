import { Grid2, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { getWatchlistQuery } from '../../../queries/watchlistsQueries';
import { MovieCard } from '../../../components/MovieCard/MovieCard';
import { MovieList } from '../../../components/MovieList';

export function Watchist() {
  const { data: watchlist } = useQuery(getWatchlistQuery());

  return <MovieList movies={watchlist?.movies} />;
}
