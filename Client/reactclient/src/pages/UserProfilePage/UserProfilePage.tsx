import { useMutation, useQuery } from '@tanstack/react-query';
import { getUserProfileQuery } from '../../queries/profilesQueries';
import { Button, CircularProgress, Grid2, Typography } from '@mui/material';
import { Watchlist } from './components/Watchlist';
import { changePassword, sendConfirmationEmail } from '../../api/userService';
import { useState } from 'react';

export function UserProfilePage() {
  const [hasSent, setHasSent] = useState(false);
  const { data: profile } = useQuery(getUserProfileQuery());

  const {
    mutateAsync: sendConfirmAsync,
    isLoading,
    isSuccess,
  } = useMutation({
    mutationFn: () => sendConfirmationEmail(),
  });

  const { mutateAsync: changeAsync } = useMutation({
    mutationFn: () => changePassword(),
  });

  const handleConfirmEmail = () => {
    sendConfirmAsync().then(() => setHasSent(true));
  };

  const handleChangePassword = () => {
    changeAsync();
  };

  return (
    <Grid2 container direction={'column'} spacing={2} alignItems={'center'}>
      <Typography variant='h4' color='inherit'>
        {profile?.username}
      </Typography>
      <Watchlist />
      <Button onClick={handleConfirmEmail} variant='contained'>
        Confirm Email
      </Button>
      {hasSent ? (
        <Typography color='error'>
          {isLoading ? (
            <CircularProgress />
          ) : isSuccess ? (
            'Check your email'
          ) : (
            'Something went wrong'
          )}
        </Typography>
      ) : null}
      <Button onClick={handleChangePassword} variant='contained'>
        Change Password
      </Button>
    </Grid2>
  );
}
