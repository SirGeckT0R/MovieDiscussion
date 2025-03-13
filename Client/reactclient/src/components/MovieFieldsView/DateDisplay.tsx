import { Typography } from '@mui/material';

type Props = {
  date: Date;
  showTime?: boolean;
};

export function DateDisplay({ date, showTime }: Props) {
  return (
    <Typography variant='body1' sx={{ fontSize: '1.25rem' }} display={'inline'}>
      {date.toLocaleDateString('en-US', {
        day: 'numeric',
        month: 'short',
        year: 'numeric',
      })}
      {showTime
        ? ' ' +
          date.toLocaleTimeString('en-US', {
            hour: 'numeric',
            minute: '2-digit',
          })
        : null}
    </Typography>
  );
}
