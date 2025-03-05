import { Grid2, Typography } from '@mui/material';
import { Watchlist } from './components/Watchlist';
import { ConfirmEmailAction } from './components/ConfirmEmailAction';
import { ChangePasswordAction } from './components/ChangePasswordAction';
import { useAuth } from '../../hooks/useAuth';

export function UserProfilePage() {
  const { user: profile } = useAuth();

  return (
    <Grid2 container direction={'column'} spacing={2} alignItems={'center'}>
      <Typography variant='h4' color='inherit'>
        {profile?.username}
      </Typography>
      <Watchlist />
      {!profile.isEmailConfirmed && <ConfirmEmailAction />}
      <ChangePasswordAction />
    </Grid2>
  );
}
