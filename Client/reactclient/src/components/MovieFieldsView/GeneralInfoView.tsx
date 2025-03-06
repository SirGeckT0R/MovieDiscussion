import { Typography, Rating, Stack } from '@mui/material';
import { Role } from '../../types/user';
import { MovieAdminActions } from '../../pages/MoviePage/components/MovieAdminActions';
import { Movie } from '../../types/movie';
import { useAuth } from '../../hooks/useAuth';

type Props = {
  movie: Movie | undefined;
  allowActions?: boolean;
};

export function GeneralInfoView({ movie, allowActions = true }: Props) {
  const { user } = useAuth();

  return (
    <Stack spacing={2}>
      <Typography variant='h2' align='left' color='info' fontWeight={'bold'}>
        {movie?.title}
      </Typography>
      {user.role === Role.Admin && allowActions && (
        <MovieAdminActions id={movie?.id} image={movie?.image} />
      )}

      <Rating
        value={movie ? movie.rating / 2 : 0}
        max={5}
        precision={0.5}
        readOnly
      />
      <Typography
        variant='h3'
        align='left'
        color='info'
        sx={{ wordBreak: 'break-word' }}>
        {movie?.description}
      </Typography>
    </Stack>
  );
}
