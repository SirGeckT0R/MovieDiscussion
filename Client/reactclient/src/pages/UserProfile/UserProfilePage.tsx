import { useQuery } from '@tanstack/react-query';
import { getUserProfileQuery } from '../../queries/profilesQueries';
import { Grid2, Typography } from '@mui/material';
import { Watchist } from './components/Watchlist';

export function UserProfilePage() {
  const { data: profile } = useQuery(getUserProfileQuery());

  return (
    <Grid2 container>
      <Typography variant='h4' color='inherit'>
        {profile?.username}
      </Typography>
      <Watchist />
    </Grid2>
  );
}
