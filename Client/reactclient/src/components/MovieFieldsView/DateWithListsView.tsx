import { Stack, Typography, Box } from '@mui/material';
import { CrewMembersView } from './CrewMembersView';
import { DateDisplay } from './DateDisplay';
import { MovieGenresView } from './MovieGenresView';
import { Movie } from '../../types/movie';

export function DateWithListsView({ movie }: { movie: Movie | undefined }) {
  return (
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
      <Typography variant='h6'>Crew:</Typography>
      <CrewMembersView crew={movie?.crewMembers} />
    </Stack>
  );
}
