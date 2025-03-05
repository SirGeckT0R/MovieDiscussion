import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { deleteReview } from '../../../api/reviewService';

export function DeleteReviewAction({
  id,
  queryInvalidator,
}: {
  id: string | undefined;
  queryInvalidator: () => void;
}) {
  const { mutateAsync } = useMutation({
    mutationFn: () => deleteReview(id ?? ''),
  });

  const handleDeleteReview = () => {
    mutateAsync().then(queryInvalidator);
  };

  return (
    <Button color='error' variant='contained' onClick={handleDeleteReview}>
      Delete
    </Button>
  );
}
