import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { changePassword } from '../../../api/userService';

export function ChangePasswordAction() {
  const { mutateAsync } = useMutation({
    mutationFn: () => changePassword(),
  });

  const handleChangePassword = () => {
    mutateAsync();
  };

  return (
    <Button onClick={handleChangePassword} variant='contained'>
      Change Password
    </Button>
  );
}
