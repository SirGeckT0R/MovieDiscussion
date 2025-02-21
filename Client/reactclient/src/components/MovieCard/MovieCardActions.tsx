import { VisibilityTwoTone } from '@mui/icons-material';
import { Checkbox, IconButton, Tooltip } from '@mui/material';
import { Movie } from '../../types/movie';
import { useState } from 'react';

export function MovieCardActions({ movie }: { movie: Movie }) {
  const [loading, setLoading] = useState(false);

  function handleClick(event: React.ChangeEvent<HTMLInputElement>) {
    console.log(event.target.checked + movie.id);
    setLoading(true);
    setTimeout(() => setLoading(false), 500);
  }
  return (
    <>
      <Tooltip title='Add to watchlist'>
        <IconButton loading={loading}>
          <Checkbox
            icon={<VisibilityTwoTone />}
            checkedIcon={<VisibilityTwoTone htmlColor='green' />}
            onChange={handleClick}
            sx={{ display: loading ? 'none' : 'block' }}
          />
        </IconButton>
      </Tooltip>
    </>
  );
}
