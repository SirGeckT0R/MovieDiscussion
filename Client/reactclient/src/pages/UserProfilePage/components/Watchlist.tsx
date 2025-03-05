import { useMutation, useQuery } from '@tanstack/react-query';
import { getWatchlistQuery } from '../../../queries/watchlistsQueries';
import { Card, Grid2 } from '@mui/material';
import { NavLink } from 'react-router-dom';
import { MovieCardInfo } from '../../../components/MovieCard/MovieCardInfo';
import { useMovieCardStyles } from '../../../components/MovieCard/styles/useMovieCardStyles';
import { DeleteFromWatchlist } from './DeleteFromWatchlist';
import { manageMovieInWatchlist } from '../../../api/watchlistService';
import {
  ManageMovieInWatchlist,
  WatchlistAction,
} from '../../../types/watchlist';
import { queryClient } from '../../../api/global';

export function Watchlist() {
  const classes = useMovieCardStyles();

  const watchlistQuery = getWatchlistQuery();
  const { data: watchlist } = useQuery(watchlistQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(watchlistQuery.queryKey);
  };

  const { mutateAsync, isLoading } = useMutation({
    mutationFn: (values: ManageMovieInWatchlist) =>
      manageMovieInWatchlist(values),
  });

  const handleDelete = (id: string) => {
    const values: ManageMovieInWatchlist = {
      movieId: id,
      action: WatchlistAction.Remove as number,
    };

    mutateAsync(values).then(queryInvalidator);
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
            handleClick={() => handleDelete(movie.id)}
          />
        </Card>
      ))}
    </Grid2>
  );
}
