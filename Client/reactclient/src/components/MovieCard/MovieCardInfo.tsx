import { Box, CardContent, CardMedia, Rating, Typography } from '@mui/material';
import { Movie } from '../../types/movie';
import { useMovieCardStyles } from './styles/useMovieCardStyles';
import { ImageSharp } from '@mui/icons-material';
import { emptyImageStyle } from './styles/emptyImageStyle';

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
        <ImageSharp sx={emptyImageStyle} />
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
