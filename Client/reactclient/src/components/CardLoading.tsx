import { Box, Grid2, Skeleton } from '@mui/material';

export function CardLoader({ amount }: { amount: number }) {
  const cards = Array.from({ length: amount }, (_, i) => (
    <Box key={i}>
      <Skeleton
        variant='rectangular'
        width={360}
        height={600}
        animation='wave'
      />
      <Box sx={{ pt: 0.5 }}>
        <Skeleton animation='wave' />
        <Skeleton width='60%' animation='wave' />
      </Box>
    </Box>
  ));

  return (
    <Grid2 container spacing={2} columns={3}>
      {cards}
    </Grid2>
  );
}
