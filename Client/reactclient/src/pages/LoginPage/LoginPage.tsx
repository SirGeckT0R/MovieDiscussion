import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { NavLink, useNavigate } from 'react-router-dom';
import { Button, Stack, TextField } from '@mui/material';
import { fetchLogin } from '../../api/userService';
import { useAuth } from '../../hooks/useAuth';
import { LoginRequest } from '../../types/user';

export function LoginPage() {
  const navigate = useNavigate();
  const { authenticate } = useAuth();

  const { register, handleSubmit } = useForm<LoginRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: LoginRequest) => fetchLogin(values),
  });

  const onLogin = (formBody: LoginRequest) => {
    mutateAsync(formBody)
      .then(() => authenticate())
      .then(() => navigate('/movies'));
  };

  return (
    <Stack direction={'column'} spacing={2} sx={{ width: 500 }}>
      <form onSubmit={handleSubmit(onLogin)}>
        <Stack direction={'column'} spacing={2}>
          <TextField
            type='email'
            id='emailInput'
            label='Email'
            {...register('email')}
            required
          />
          <TextField
            type='password'
            id='passwordInput'
            label='Password'
            {...register('password')}
            required
          />
          <Button type='submit' variant='contained'>
            Login
          </Button>
        </Stack>
      </form>
      <NavLink to='/auth/password/forgot'>Forgot password?</NavLink>
    </Stack>
  );
}
