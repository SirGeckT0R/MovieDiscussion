import { useMutation } from '@tanstack/react-query';
import { forgotPassword } from '../../api/userService';
import { ForgotPasswordRequest } from '../../types/user';
import { useForm } from 'react-hook-form';
import {
  Button,
  CircularProgress,
  Stack,
  TextField,
  Typography,
} from '@mui/material';
import { useState } from 'react';

export function ForgotPasswordPage() {
  const [hasSubmitted, setHasSubmitted] = useState(false);
  const { register, handleSubmit } = useForm<ForgotPasswordRequest>();

  const { mutateAsync, isSuccess, isLoading } = useMutation({
    mutationFn: (values: ForgotPasswordRequest) => forgotPassword(values),
  });

  const onForgot = (values: ForgotPasswordRequest) => {
    mutateAsync(values).then(() => setHasSubmitted(true));
  };

  return (
    <>
      <form onSubmit={handleSubmit(onForgot)}>
        <Stack spacing={2}>
          <TextField
            type='text'
            id='emailInput'
            label='Email'
            {...register('email')}
            required
          />
          <Button type='submit' variant='contained'>
            Submit
          </Button>
        </Stack>
      </form>
      {hasSubmitted ? (
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
    </>
  );
}
