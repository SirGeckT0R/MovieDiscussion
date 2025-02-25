import { useQuery } from '@tanstack/react-query';
import { fetchMovies } from '../api/movieService';

export function useMovies(pageIndex: number, name: string | null) {
  return useQuery({
    queryKey: ['movies', name + '' + pageIndex],
    queryFn: async () => await fetchMovies(pageIndex, name),
  });
}
