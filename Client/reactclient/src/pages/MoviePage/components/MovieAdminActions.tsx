import { Stack, Button } from '@mui/material';
import { NavLink, useNavigate } from 'react-router-dom';
import { deleteMovie } from '../../../api/movieService';
import { useMutation } from '@tanstack/react-query';

type Props = {
  id: string | undefined;
  image: string | undefined;
};

export function MovieAdminActions({ id, image }: Props) {
  const navigate = useNavigate();

  const { mutateAsync } = useMutation({
    mutationFn: () => deleteMovie({ id: id ?? '', image: image ?? '' }),
  });

  const handleDeleteMovie = () => {
    mutateAsync().then(() => navigate('/movies'));
  };

  return (
    <Stack direction={'row'} spacing={2}>
      <NavLink to='edit'>
        <Button color='warning' variant='contained'>
          Edit
        </Button>
      </NavLink>
      <Button color='error' variant='contained' onClick={handleDeleteMovie}>
        Delete
      </Button>
    </Stack>
  );
}
