import { useQuery } from '@tanstack/react-query';
import { MovieList } from '../../components/MovieList';
import { getMoviesQuery } from '../../queries/moviesQueries';

export function MoviesPage() {
  const { data: movies } = useQuery(getMoviesQuery());

  return (
    <div>
      <MovieList movies={movies!} />
    </div>
  );
}
