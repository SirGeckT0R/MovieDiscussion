import {
  FormControl,
  FormLabel,
  Grid2,
  Rating,
  TextField,
  Typography,
} from '@mui/material';
import { Box, Stack } from '@mui/system';
import { getMovieQuery } from '../../queries/moviesQueries';
import { useParams } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import { CrewMembersView } from '../../components/CrewMembersView';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';
import { DateDisplay } from '../../components/DateDisplay';
import { ReviewView } from './components/ReviewView';
import { MovieAdminActions } from './components/MovieAdminActions';
import { MovieGenresView } from '../../components/MovieGenresView';
import { ImageSharp } from '@mui/icons-material';

export function MoviePage() {
  const { user } = useAuth();

  const { id } = useParams();
  const { data: movie } = useQuery(getMovieQuery(id!));

  return (
    <Grid2 container direction={'column'} spacing={2}>
      <Grid2 container spacing={2}>
        <Grid2 size='auto'>
          {movie?.image ? (
            <Box
              width='600px'
              component='img'
              src={`${import.meta.env.VITE_IMAGES_HOST}/${movie.image}`}
              alt={`${movie.title} - cover`}
              title={`${movie.title} - cover`}
              sx={{ objectFit: 'contain' }}
            />
          ) : (
            <ImageSharp
              sx={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                cursor: 'pointer',
                height: 300,
                pb: 28,
                color: 'grey',
              }}
            />
          )}
        </Grid2>
        <Grid2
          container
          direction={'column'}
          alignItems={'start'}
          color='info'
          spacing={2}
          size='grow'>
          <Stack>
            <Typography
              variant='h2'
              align='left'
              color='info'
              fontWeight={'bold'}>
              {movie?.title}
            </Typography>
            {user.role == Role.Admin ? (
              <MovieAdminActions id={id} image={movie?.image} />
            ) : (
              <></>
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
          <Stack direction={'row'} spacing={20} width={'100%'}>
            <Stack alignItems={'flex-start'}>
              <Typography variant='h5' color='info'>
                Release Date:&nbsp;
                <DateDisplay date={new Date(movie?.releaseDate ?? '')} />
              </Typography>
              <Typography variant='h6'>Genres:</Typography>
              <Box
                sx={{
                  padding: '0 20px',
                }}>
                <MovieGenresView genres={movie?.genres} />
              </Box>
              <Typography variant='h5' color='info'>
                Crew:
              </Typography>
              <CrewMembersView crew={movie?.crewMembers} />
            </Stack>
            <ReviewView movieId={id} />
          </Stack>
        </Grid2>
      </Grid2>
    </Grid2>
  );
}
