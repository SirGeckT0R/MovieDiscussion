import { Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { manageMovieApproval } from '../../../api/movieService';
import { ManageMovieApprovalRequest } from '../../../types/movie';

type Props = {
  id: string | undefined;
  queryInvalidator: () => void;
};

export function MovieApprovalActions({ id, queryInvalidator }: Props) {
  const { mutateAsync } = useMutation({
    mutationFn: (values: ManageMovieApprovalRequest) =>
      manageMovieApproval(values),
  });

  const handleApprovalMovie = (
    movieId: string | undefined,
    shouldApprove: boolean
  ) => {
    mutateAsync({ movieId: movieId ?? '', shouldApprove }).then(
      queryInvalidator
    );
  };

  return (
    <>
      <Button
        color='success'
        variant='contained'
        onClick={() => handleApprovalMovie(id, true)}>
        Approve
      </Button>
      <Button
        color='error'
        variant='contained'
        onClick={() => handleApprovalMovie(id, false)}>
        Deny
      </Button>
    </>
  );
}
