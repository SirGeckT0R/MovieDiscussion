import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { subscribeToDiscussion } from '../../../api/discussionsService';

type Props = {
  id: string | undefined;
  queryInvalidator: () => void;
};

export function SubscribeDiscussionAction({ id, queryInvalidator }: Props) {
  const { mutateAsync } = useMutation({
    mutationFn: (id: string) => subscribeToDiscussion(id),
  });

  const handleSubcription = () => {
    mutateAsync(id ?? '').then(queryInvalidator);
  };

  return (
    <Button variant='contained' color='warning' onClick={handleSubcription}>
      Get Notification
    </Button>
  );
}
