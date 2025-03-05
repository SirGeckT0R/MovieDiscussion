import { Box, CardContent, CardMedia, Rating, Typography } from '@mui/material';
import { Movie } from '../../types/movie';
import { useMovieCardStyles } from './styles/useMovieCardStyles';
import { ImageSharp } from '@mui/icons-material';

export function MovieCardInfo({ movie }: { movie: Movie }) {
  const classes = useMovieCardStyles();

  return (
    <>
      {movie.image ? (
        <CardMedia
          className={classes.media}
          component='img'
          image={`${import.meta.env.VITE_IMAGES_HOST}/${movie.image}`}
          alt={`${movie.title} - cover`}
          title={`${movie.title} - cover`}
          sx={{ objectFit: 'cover', width: 360, height: 300 }}
        />
      ) : (
        <ImageSharp
          sx={{
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            cursor: 'pointer',
            width: 360,
            height: 300,
            pb: 28,
            color: 'grey',
          }}
        />
      )}
      <Box className={classes.overlay} height={200}>
        <CardContent className={classes.content}>
          <Typography
            gutterBottom
            variant='h6'
            component='div'
            color='info'
            fontWeight={'bold'}
            textAlign={'center'}>
            {movie.title}
          </Typography>
          <Rating value={movie.rating / 2} max={5} precision={0.5} readOnly />
        </CardContent>
      </Box>
    </>
  );
}
