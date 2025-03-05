import { useQuery } from '@tanstack/react-query';
import { fetchMovies } from '../api/movieService';
import { MovieFilters } from '../types/movie';

export function useMovies(pageIndex: number, filters: MovieFilters | null) {
  return useQuery({
    queryKey: [
      'movies',
      filters?.name +
        '' +
        filters?.genres +
        '' +
        filters?.crewMember +
        '' +
        pageIndex,
    ],
    queryFn: async () => await fetchMovies(pageIndex, filters),
  });
}
