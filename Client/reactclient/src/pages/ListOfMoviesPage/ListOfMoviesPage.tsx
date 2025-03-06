import { useState } from 'react';
import { Box, Button, Grid2, Pagination, Stack } from '@mui/material';
import { CardLoader } from '../../components/Loaders/CardLoader';
import { MovieFilters } from '../../types/movie';
import { MovieFiltersForm } from './components/MovieFiltersForm';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';
import { MovieList } from '../../components/MovieList/MovieList';
import { useQuery } from '@tanstack/react-query';
import { getListOfMoviesQuery } from '../../queries/moviesQueries';

export function ListOfMoviesPage() {
  const { user } = useAuth();

  const [pageIndex, setPageIndex] = useState(1);
  const [filters, setFilters] = useState<MovieFilters>({
    name: '',
    genres: [],
    crewMember: '',
  });

  const moviesQuery = getListOfMoviesQuery(pageIndex, filters);
  const { data: movies, isLoading } = useQuery(moviesQuery);

  const handlePageClick = (_: unknown, value: number) => {
    setPageIndex(value);
  };

  return (
    <Stack spacing={2} alignItems={'center'}>
      {user.role === Role.Admin && (
        <Button
          component={NavLink}
          to='/movies/not-approved'
          variant='contained'
          color='warning'>
          Not Approved
        </Button>
      )}
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
    </Stack>
  );
}
