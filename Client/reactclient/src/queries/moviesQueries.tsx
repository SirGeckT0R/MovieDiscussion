import {
  fetchMovie,
  fetchMovies,
  fetchNotApprovedMovies,
} from '../api/movieService';
import { MovieFilters } from '../types/movie';

export const getListOfMoviesQuery = (
  pageIndex: number = 1,
  filters: MovieFilters | null = null
) => ({
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

export const getNotApprovedMoviesQuery = () => ({
  queryKey: ['movies/not-approved'],
  queryFn: async () => await fetchNotApprovedMovies(),
});

export const getMovieQuery = (id: string | undefined = '') => ({
  queryKey: ['movies', id],
  queryFn: async () => await fetchMovie(id),
});
