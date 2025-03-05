import { Stack, Typography } from '@mui/material';
import { CreatePersonInput } from '../../components/Inputs/CreatePersonInput';
import { UpdatePersonForm } from './components/UpdatePersonForm';
import { DeletePersonForm } from './components/DeletePersonForm';

export function ModifyPersonPage() {
  return (
    <Stack spacing={5} width={500}>
      <CreatePersonInput />
      <Typography variant='h5'>Modify existing ones</Typography>
      <UpdatePersonForm />
      <Typography variant='h5'>Delete</Typography>
      <DeletePersonForm />
    </Stack>
  );
}
