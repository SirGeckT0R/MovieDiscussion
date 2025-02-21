import { fetchMovies } from '../api/movieService';

export const getMoviesQuery = () => ({
  queryKey: ['movies'],
  queryFn: async () => await fetchMovies(),
});

export const getMoviesQueryLong = () => ({
  queryKey: ['movies'],
  queryFn: async () => {
    await new Promise((res) => setTimeout(res, 4000));
    return await fetchMovies();
  },
});
