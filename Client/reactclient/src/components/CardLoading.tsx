import { Box, Grid2, Skeleton } from '@mui/material';

export function CardLoader() {
  return (
    <Grid2 container spacing={2} columns={3}>
      <div>
        <Skeleton
          variant='rectangular'
          width={360}
          height={118}
          animation='wave'
        />
        <Box sx={{ pt: 0.5 }}>
          <Skeleton animation='wave' />
          <Skeleton width='60%' animation='wave' />
        </Box>
      </div>
      <div>
        <Skeleton
          variant='rectangular'
          width={360}
          height={118}
          animation='wave'
        />
        <Box sx={{ pt: 0.5 }}>
          <Skeleton animation='wave' />
          <Skeleton width='60%' animation='wave' />
        </Box>
      </div>
    </Grid2>
  );
}
