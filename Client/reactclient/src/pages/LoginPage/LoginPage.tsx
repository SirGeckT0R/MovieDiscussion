import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Button, Grid2, TextField } from '@mui/material';
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

  const onSubmit = (formBody: LoginRequest) => {
    mutateAsync(formBody)
      .then(async () => await authenticate())
      .then(() => navigate('/movies'));
  };

  return (
    <div style={{ width: 500 }}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Grid2 container direction={'column'} gap={'20px'}>
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
        </Grid2>
      </form>
    </div>
  );
}
