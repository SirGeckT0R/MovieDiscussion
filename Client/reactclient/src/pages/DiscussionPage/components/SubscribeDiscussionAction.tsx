import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { subscribeToDiscussion } from '../../../api/discussionsService';

export function SubscribeDiscussionAction({ id }: { id: string | undefined }) {
  const { mutateAsync } = useMutation({
    mutationFn: (id: string) => subscribeToDiscussion(id),
  });

  const handleSubcription = () => {
    mutateAsync(id ?? '');
  };

  return (
    <Button variant='contained' color='warning' onClick={handleSubcription}>
      Get Notification
    </Button>
  );
}
