import { RemoveCircleOutline } from '@mui/icons-material';
import { Tooltip, IconButton, Checkbox } from '@mui/material';

type Props = {
  isLoading: boolean;
  handleClick: () => void;
};

export function DeleteFromWatchlist({ isLoading, handleClick }: Props) {
  return (
    <Tooltip title='Remove from watchlist'>
      <IconButton loading={isLoading}>
        <Checkbox
          icon={<RemoveCircleOutline />}
          checkedIcon={<RemoveCircleOutline htmlColor='black' />}
          onChange={handleClick}
          sx={{ display: isLoading ? 'none' : 'block' }}
        />
      </IconButton>
    </Tooltip>
  );
}
