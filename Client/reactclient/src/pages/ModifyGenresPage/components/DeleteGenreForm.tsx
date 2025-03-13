import { Stack, Typography, Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { deleteGenre } from '../../../api/genreService';
import { CustomSelectInput } from '../../../components/Inputs/MultipleSelectInput';
import { Genre, DeleteGenreRequest } from '../../../types/genre';

type Props = {
  queryInvalidator: () => void;
  genres: Genre[] | undefined;
};

export function DeleteGenreForm({ queryInvalidator, genres }: Props) {
  const { control, handleSubmit } = useForm<DeleteGenreRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: DeleteGenreRequest) => deleteGenre(values),
  });

  const onDeleteSubmit = (formBody: DeleteGenreRequest) => {
    mutateAsync(formBody).then(queryInvalidator);
  };

  return (
    <form onSubmit={handleSubmit(onDeleteSubmit)}>
      <Stack spacing={2}>
        <Typography variant='h5'>Delete</Typography>
        <CustomSelectInput
          isMultiple={false}
          label='Genre'
          options={genres}
          control={control}
          inputName='id'
        />
        <Button type='submit' variant='contained'>
          Delete
        </Button>
      </Stack>
    </form>
  );
}
