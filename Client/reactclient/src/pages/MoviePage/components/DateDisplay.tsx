import { Typography } from '@mui/material';

type Props = {
  date: Date;
  hideTime?: boolean;
};

export function DateDisplay({ date, hideTime }: Props) {
  return (
    <Typography variant='h4'>
      {date.toLocaleDateString('en-US', {
        day: 'numeric',
        month: 'short',
        year: 'numeric',
      })}
      {hideTime
        ? ' ' +
          date.toLocaleTimeString('en-US', {
            hour: 'numeric',
            minute: '2-digit',
          })
        : null}
    </Typography>
  );
}
