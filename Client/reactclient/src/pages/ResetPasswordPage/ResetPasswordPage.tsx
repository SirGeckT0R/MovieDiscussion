import { useMutation } from '@tanstack/react-query';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { resetPassword } from '../../api/userService';
import { Button, Stack, TextField } from '@mui/material';
import { useForm } from 'react-hook-form';
import { ResetPasswordRequest } from '../../types/user';

export function ResetPasswordPage() {
  const navigate = useNavigate();

  const [searchParams] = useSearchParams();
  const email = searchParams.get('email');
  const token = searchParams.get('token');

  const { mutateAsync } = useMutation({
    mutationFn: (values: ResetPasswordRequest) => resetPassword(values),
  });

  const { register, handleSubmit } = useForm<ResetPasswordRequest>({
    defaultValues: { email, token, newPassword: '' },
  });

  const onSubmit = (values: ResetPasswordRequest) => {
    mutateAsync(values).then(() => navigate('/login'));
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Stack spacing={2}>
        <TextField
          type='text'
          id='newPasswordInput'
          label='New Password'
          {...register('newPassword')}
          required
        />
        <Button type='submit' variant='contained'>
          Reset
        </Button>
      </Stack>
    </form>
  );
}
