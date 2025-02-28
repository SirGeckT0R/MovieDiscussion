import { RemoveCircleOutline } from '@mui/icons-material';
import { Tooltip, IconButton, Checkbox } from '@mui/material';

export function DeleteFromWatchlist({
  isLoading,
  handleClick,
}: {
  isLoading: boolean;
  handleClick: () => void;
}) {
  return (
    <>
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
    </>
  );
}
