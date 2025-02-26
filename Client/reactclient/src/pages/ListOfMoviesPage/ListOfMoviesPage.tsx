import { MovieList } from '../../components/MovieList';
import { useState } from 'react';
import { Pagination } from '@mui/material';
import { useMovies } from '../../hooks/useMovies';
import { useQuery } from '@tanstack/react-query';
import { getWatchlistQuery } from '../../queries/watchlistsQueries';

export function ListOfMoviesPage() {
  const [pageIndex, setPageIndex] = useState(1);

  const { data: movies } = useMovies(pageIndex, '');
  const { data: watchlist } = useQuery(getWatchlistQuery());

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return (
    <div>
      <MovieList
        movies={movies?.items}
        watchlist={watchlist?.movies.map((movie) => movie.id)}
      />
      <Pagination
        count={movies?.totalPages}
        onChange={handlePageClick}
        sx={{ mr: 50 }}
      />
    </div>
  );
}
