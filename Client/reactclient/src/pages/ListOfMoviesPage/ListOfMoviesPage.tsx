import { MovieList } from '../../components/MovieList';
import { useState } from 'react';
import { Pagination } from '@mui/material';
import { useMovies } from '../../hooks/useMovies';

export function ListOfMoviesPage() {
  const [pageIndex, setPageIndex] = useState(1);

  const { data: movies } = useMovies(pageIndex, '');

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return (
    <div>
      <MovieList movies={movies?.items} />
      <Pagination
        count={movies?.totalPages}
        onChange={handlePageClick}
        sx={{ mr: 50 }}
      />
    </div>
  );
}
