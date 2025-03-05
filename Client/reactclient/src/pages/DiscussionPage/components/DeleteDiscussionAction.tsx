import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { deleteDiscussion } from '../../../api/discussionsService';

export function DeleteDiscussionAction({ id }: { id: string | undefined }) {
  const navigate = useNavigate();

  const { mutateAsync } = useMutation({
    mutationFn: (id: string) => deleteDiscussion(id),
  });

  const handleDiscussionDelete = () => {
    mutateAsync(id ?? '').then(() => navigate('/discussions'));
  };

  return (
    <Button color='error' variant='contained' onClick={handleDiscussionDelete}>
      Delete
    </Button>
  );
}
