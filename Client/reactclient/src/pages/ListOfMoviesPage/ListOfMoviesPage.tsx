import { MovieList } from '../../components/MovieList';
import { useState } from 'react';
import { Pagination, Stack } from '@mui/material';
import { useMovies } from '../../hooks/useMovies';
import { CardLoader } from '../../components/CardLoading';

export function ListOfMoviesPage() {
  const [pageIndex, setPageIndex] = useState(1);

  const { data: movies, isLoading } = useMovies(pageIndex, '');

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return isLoading ? (
    <CardLoader amount={3} />
  ) : (
    <Stack direction={'column'} spacing={4} alignItems={'center'}>
      <MovieList movies={movies?.items} />
      <Pagination
        count={movies?.totalPages}
        onChange={handlePageClick}
        sx={{ mr: 50 }}
      />
    </Stack>
  );
}
