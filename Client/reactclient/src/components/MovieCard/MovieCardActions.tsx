import { VisibilityTwoTone } from '@mui/icons-material';
import { Checkbox, IconButton, Tooltip } from '@mui/material';
import { Movie } from '../../types/movie';
import { useMutation } from '@tanstack/react-query';
import { manageMovieInWatchlist } from '../../api/watchlistService';
import { ManageMovieInWatchlist, WatchlistAction } from '../../types/watchlist';

type Props = {
  movie: Movie;
  isInWatchlist: boolean;
};

export function MovieCardActions({ movie, isInWatchlist = false }: Props) {
  const { mutateAsync, isLoading } = useMutation({
    mutationFn: (values: ManageMovieInWatchlist) =>
      manageMovieInWatchlist(values),
  });

  const handleClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    const action = event.target.checked
      ? WatchlistAction.Add
      : WatchlistAction.Remove;

    const values: ManageMovieInWatchlist = {
      movieId: movie.id,
      action: action as number,
    };

    mutateAsync(values);
  };

  return (
    <Tooltip title='Add to watchlist'>
      <IconButton loading={isLoading}>
        <Checkbox
          defaultChecked={isInWatchlist}
          icon={<VisibilityTwoTone />}
          checkedIcon={<VisibilityTwoTone htmlColor='green' />}
          onChange={handleClick}
          sx={{ display: isLoading ? 'none' : 'initial' }}
        />
      </IconButton>
    </Tooltip>
  );
}
