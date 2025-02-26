import { VisibilityTwoTone } from '@mui/icons-material';
import { Checkbox, IconButton, Tooltip } from '@mui/material';
import { Movie } from '../../types/movie';
import { useMutation } from '@tanstack/react-query';
import { manageMovieInWatchlist } from '../../api/watchlistService';
import { ManageMovieInWatchlist, WatchlistAction } from '../../types/watchlist';

export function MovieCardActions({
  movie,
  isInWatchlist = false,
}: {
  movie: Movie;
  isInWatchlist: boolean;
}) {
  const { mutateAsync, isLoading } = useMutation({
    mutationFn: (values: ManageMovieInWatchlist) =>
      manageMovieInWatchlist(values),
  });

  function handleClick(event: React.ChangeEvent<HTMLInputElement>) {
    const action = event.target.checked
      ? WatchlistAction.Add
      : WatchlistAction.Remove;

    const values: ManageMovieInWatchlist = {
      movieId: movie.id,
      action: action as number,
    };

    mutateAsync(values);
  }

  return (
    <>
      <Tooltip title='Add to watchlist'>
        <IconButton loading={isLoading}>
          <Checkbox
            defaultChecked={isInWatchlist}
            icon={<VisibilityTwoTone />}
            checkedIcon={<VisibilityTwoTone htmlColor='green' />}
            onChange={handleClick}
            sx={{ display: isLoading ? 'none' : 'block' }}
          />
        </IconButton>
      </Tooltip>
    </>
  );
}
