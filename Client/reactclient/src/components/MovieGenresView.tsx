import { Stack, Chip } from '@mui/material';
import { Genre } from '../types/genre';

export function MovieGenresView({ genres }: { genres: Genre[] | undefined }) {
  return (
    <Stack direction={'row'} spacing={1}>
      {genres?.map((genre) => (
        <Chip
          label={genre.name}
          color='primary'
          sx={{
            fontWeight: 'bold',
          }}
          key={genre.id}
        />
      ))}
    </Stack>
  );
}
