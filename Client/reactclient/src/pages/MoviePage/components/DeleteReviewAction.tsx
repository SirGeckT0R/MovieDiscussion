import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { deleteReview } from '../../../api/reviewService';

type Props = {
  id: string | undefined;
  queryInvalidator: () => void;
};

export function DeleteReviewAction({ id, queryInvalidator }: Props) {
  const { mutateAsync } = useMutation({
    mutationFn: () => deleteReview({ id: id ?? '' }),
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
