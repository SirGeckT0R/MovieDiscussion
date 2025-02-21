import { fetchGenres } from '../api/genreService';

export const getGenresQuery = () => ({
  queryKey: ['genres'],
  queryFn: async () => await fetchGenres(),
});
