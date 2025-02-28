import { useMutation, useQuery } from '@tanstack/react-query';
import { getWatchlistQuery } from '../../../queries/watchlistsQueries';
import { Card, Divider, Grid2 } from '@mui/material';
import { NavLink } from 'react-router-dom';
import { MovieCardInfo } from '../../../components/MovieCard/MovieCardInfo';
import { useMovieCardStyles } from '../../../components/MovieCard/styles/useMovieCardStyles';
import { DeleteFromWatchlist } from './DeleteFromWatchlist';
import { manageMovieInWatchlist } from '../../../api/watchlistService';
import {
  ManageMovieInWatchlist,
  WatchlistAction,
} from '../../../types/watchlist';
import { Movie } from '../../../types/movie';

export function Watchlist() {
  const { data: watchlist } = useQuery(getWatchlistQuery());
  const classes = useMovieCardStyles();

  const { mutateAsync: manageAsync, isLoading } = useMutation({
    mutationFn: (values: ManageMovieInWatchlist) =>
      manageMovieInWatchlist(values),
  });

  const handleClick = (movie: Movie) => {
    const values: ManageMovieInWatchlist = {
      movieId: movie.id,
      action: WatchlistAction.Remove as number,
    };

    const index = watchlist?.movies.indexOf(movie) ?? -1;

    manageAsync(values).then(() => {
      if (index > -1) {
        watchlist?.movies.splice(index, 1);
      }
    });
  };

  return (
    <Grid2 container spacing={2} columns={3}>
      {watchlist?.movies?.map((movie) => (
        <Card
          className={classes.card}
          variant='outlined'
          sx={{ width: 360, height: 600 }}>
          <NavLink to={`/movies/${movie.id}`} key={movie.id}>
            <MovieCardInfo movie={movie} />
          </NavLink>
          <DeleteFromWatchlist
            isLoading={isLoading}
            handleClick={() => handleClick(movie)}
          />
        </Card>
      ))}
    </Grid2>
  );
}
