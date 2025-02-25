import { fetchMovie, fetchMovies } from '../api/movieService';

export const getListOfMoviesQuery = () => ({
  queryKey: ['movies'],
  queryFn: async () => await fetchMovies(),
});

export const getMovieQuery = (id: string | undefined = '') => ({
  queryKey: ['movies', id],
  queryFn: async () => await fetchMovie(id),
});

export const getMoviesQueryLong = () => ({
  queryKey: ['movies'],
  queryFn: async () => {
    await new Promise((res) => setTimeout(res, 4000));
    return await fetchMovies();
  },
});
