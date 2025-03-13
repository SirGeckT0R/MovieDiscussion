import { Button, Typography, CircularProgress } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { sendConfirmationEmail } from '../../../api/userService';
import { useState } from 'react';

export function ConfirmEmailAction() {
  const [hasSent, setHasSent] = useState(false);

  const { mutateAsync, isLoading, isSuccess } = useMutation({
    mutationFn: () => sendConfirmationEmail(),
  });

  const handleConfirmEmail = () => {
    mutateAsync().then(() => setHasSent(true));
  };

  return (
    <>
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
      ) : (
        ''
      )}
    </>
  );
}
