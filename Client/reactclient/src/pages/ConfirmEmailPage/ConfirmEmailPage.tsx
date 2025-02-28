import { useMutation } from '@tanstack/react-query';
import { useSearchParams } from 'react-router-dom';
import { confirmEmail } from '../../api/userService';
import { Typography } from '@mui/material';
import { useEffect } from 'react';

export function ConfirmEmailPage() {
  const [searchParams] = useSearchParams();
  const email = searchParams.get('email');
  const token = searchParams.get('token');
  const {
    mutateAsync: confirmAsync,
    isSuccess,
    isError,
    isLoading,
  } = useMutation({
    mutationFn: () => confirmEmail(email, token),
  });

  useEffect(() => {
    confirmAsync().then((response) => response.data);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return isLoading ? (
    <Typography>Loading</Typography>
  ) : (
    <>
      {isSuccess ? (
        <Typography variant='h2' color='error'>
          Email is confirmed!
        </Typography>
      ) : null}

      {isError ? (
        <Typography variant='h2' color='error'>
          Something went wrong with confirming your email
        </Typography>
      ) : null}
    </>
  );
}
