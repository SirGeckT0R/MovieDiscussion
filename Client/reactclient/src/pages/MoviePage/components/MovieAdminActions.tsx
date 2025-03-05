import { Stack, Button } from '@mui/material';
import { NavLink, useNavigate } from 'react-router-dom';
import { deleteMovie } from '../../../api/movieService';
import { useMutation } from '@tanstack/react-query';

export function MovieAdminActions({
  id,
  image,
}: {
  id: string | undefined;
  image: string | undefined;
}) {
  const navigate = useNavigate();

  const { mutateAsync } = useMutation({
    mutationFn: () => deleteMovie(id ?? '', image ?? ''),
  });

  const handleDeleteMovie = () => {
    mutateAsync().then(() => navigate('/movies'));
  };

  return (
    <Stack direction={'row'} spacing={2}>
      <NavLink to='edit'>
        <Button color='primary' variant='contained'>
          Edit
        </Button>
      </NavLink>
      <Button color='error' variant='contained' onClick={handleDeleteMovie}>
        Delete
      </Button>
    </Stack>
  );
}
