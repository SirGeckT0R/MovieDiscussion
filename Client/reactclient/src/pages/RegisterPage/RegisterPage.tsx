import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Button, Grid2, TextField } from '@mui/material';
import { useAuth } from '../../hooks/useAuth';
import { fetchRegister } from '../../api/userService';
import { RegisterRequest } from '../../types/user';

export function RegisterPage() {
  const navigate = useNavigate();
  const { authenticate } = useAuth();

  const { register, handleSubmit } = useForm<RegisterRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: RegisterRequest) => fetchRegister(values),
  });

  const onRegister = (formBody: RegisterRequest) => {
    mutateAsync(formBody)
      .then(() => authenticate())
      .then(() => navigate('/movies'));
  };

  return (
    <div style={{ width: 500 }}>
      <form onSubmit={handleSubmit(onRegister)}>
        <Grid2 container direction={'column'} gap={'20px'}>
          <TextField
            type='text'
            id='usernameInput'
            label='Username'
            {...register('username')}
            required
          />
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
            Register
          </Button>
        </Grid2>
      </form>
    </div>
  );
}
