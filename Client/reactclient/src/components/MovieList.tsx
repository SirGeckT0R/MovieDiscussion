import { Grid2 } from '@mui/material';
import { Movie } from '../types/movie';
import { MovieCard } from './MovieCard/MovieCard';
import { NavLink } from 'react-router';

export function MovieList({ movies }: { movies: Movie[] | undefined }) {
  return (
    <Grid2 container spacing={2} columns={3}>
      {movies?.map((movie) => (
        <NavLink to={`/movies/${movie.id}`} key={movie.id}>
          <MovieCard key={movie.id} movie={movie} />
        </NavLink>
      ))}
    </Grid2>
  );
}
