import { MovieList } from '../../components/MovieList';
import { useState } from 'react';
import { Box, Grid2, Pagination, Stack } from '@mui/material';
import { useMovies } from '../../hooks/useMovies';
import { CardLoader } from '../../components/CardLoading';
import { MovieFilters } from '../../types/movie';
import { MovieFiltersForm } from './components/MovieFiltersForm';

export function ListOfMoviesPage() {
  const [pageIndex, setPageIndex] = useState(1);
  const [filters, setFilters] = useState<MovieFilters>({
    name: '',
    genres: [],
    crewMember: '',
  });

  const { data: movies, isLoading } = useMovies(pageIndex, filters);

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return (
    <>
      <Grid2 container columns={2} spacing={6}>
        <Box width={900}>
          {isLoading ? (
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
          )}
        </Box>
        <MovieFiltersForm setFilters={setFilters} />
      </Grid2>
    </>
  );
}
