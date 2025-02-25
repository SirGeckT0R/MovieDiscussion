import {
  Box,
  CardContent,
  CardMedia,
  Rating,
  Stack,
  Typography,
} from '@mui/material';
import { Movie } from '../../types/movie';
import { useMovieCardStyles } from './styles/useMovieCardStyles';

export function MovieCardInfo({ movie }: { movie: Movie }) {
  const classes = useMovieCardStyles();
  return (
    <>
      {movie.image && (
        <CardMedia
          className={classes.media}
          component='img'
          image={`${import.meta.env.VITE_IMAGES_HOST}/${movie.image}`}
          alt={`${movie.title} - cover`}
          title={`${movie.title} - cover`}
          sx={{ objectFit: 'cover' }}
        />
      )}
      <div className={classes.overlay}>
        <CardContent className={classes.content}>
          <Box sx={{ p: 2 }}>
            <Stack
              sx={{ justifyContent: 'space-between', alignItems: 'center' }}>
              <Typography
                gutterBottom
                variant='h5'
                component='div'
                color='info'>
                {movie.title}
              </Typography>
              <Rating value={movie.rating} max={10} precision={0.5} readOnly />
            </Stack>
            <Typography variant='body2' sx={{ color: 'text.secondary' }}>
              {movie.description}
            </Typography>
          </Box>
        </CardContent>
      </div>
    </>
  );
}
