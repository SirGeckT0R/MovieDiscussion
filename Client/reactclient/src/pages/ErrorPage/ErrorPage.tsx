import { Box, Container, Grid2, Typography } from '@mui/material';

export function ErrorPage() {
  return (
    <Container maxWidth='xl' component={'section'} sx={{ mt: 20 }}>
      <Grid2 container flexDirection={'column'} alignItems={'center'}>
        <Box component='img' src='/404.jpg' sx={{ height: '60vh' }} />
        <Typography variant='h3' component='h1'>
          Page not found
        </Typography>
      </Grid2>
    </Container>
  );
}
