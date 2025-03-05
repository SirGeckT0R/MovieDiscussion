import {
  fetchMovie,
  fetchMovies,
  fetchNotApprovedMovies,
} from '../api/movieService';

export const getListOfMoviesQuery = () => ({
  queryKey: ['movies'],
  queryFn: async () => await fetchMovies(),
});

export const getNotApprovedMoviesQuery = () => ({
  queryKey: ['movies/not-approved'],
  queryFn: async () => await fetchNotApprovedMovies(),
});

export const getMovieQuery = (id: string | undefined = '') => ({
  queryKey: ['movies', id],
  queryFn: async () => await fetchMovie(id),
});
