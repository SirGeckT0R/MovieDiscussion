import { Stack, Typography, TextField, Button } from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { updateGenre } from '../../../api/genreService';
import { CustomSelectInput } from '../../../components/Inputs/MultipleSelectInput';
import { Genre, UpdateGenreRequest } from '../../../types/genre';

type Props = {
  queryInvalidator: () => void;
  genres: Genre[] | undefined;
};

export function UpdateGenreForm({ queryInvalidator, genres }: Props) {
  const { register, control, handleSubmit } = useForm<UpdateGenreRequest>();

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdateGenreRequest) => updateGenre(values),
  });

  const onUpdateSubmit = (formBody: UpdateGenreRequest) => {
    mutateAsync(formBody).then(queryInvalidator);
  };

  return (
    <form onSubmit={handleSubmit(onUpdateSubmit)}>
      <Stack spacing={2}>
        <Typography variant='h5'>Modify existing ones</Typography>
        <CustomSelectInput
          isMultiple={false}
          label='Genre'
          options={genres}
          control={control}
          inputName='id'
        />
        <TextField
          type='text'
          id='nameInput'
          label='New Name'
          {...register('name')}
          required
        />
        <Button type='submit' variant='contained'>
          Update
        </Button>
      </Stack>
    </form>
  );
}
