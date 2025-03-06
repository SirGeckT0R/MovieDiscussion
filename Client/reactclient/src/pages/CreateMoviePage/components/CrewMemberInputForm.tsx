import { Backdrop, Button, MenuItem, Stack, TextField } from '@mui/material';
import { Dispatch, SetStateAction, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { CrewMember, CrewRole, crewRoles } from '../../../types/movie';
import { fetchPeople } from '../../../api/peopleService';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';
import { CreatePersonInput } from '../../../components/Inputs/CreatePersonInput';

type Props = {
  crewState: {
    crew: CrewMember[];
    setCrew: Dispatch<SetStateAction<CrewMember[]>>;
  };
};

export function CrewMemberInputForm({ crewState }: Props) {
  const [open, setOpen] = useState(false);
  const [addedCrewMemberName, setAddedCrewMemberName] = useState('');

  const { handleSubmit, control } = useForm<CrewMember>();

  const onCrewSubmit = (formBody: CrewMember) => {
    formBody.fullName = addedCrewMemberName;
    crewState.setCrew((crew) => crew.concat(formBody));
  };

  return (
    <>
      <Backdrop
        sx={(theme) => ({ zIndex: theme.zIndex.drawer + 1 })}
        open={open}>
        <Stack spacing={2} direction={'row'}>
          <Button variant='contained' onClick={() => setOpen(false)}>
            Exit
          </Button>
          <CreatePersonInput />
        </Stack>
      </Backdrop>
      <form onSubmit={handleSubmit(onCrewSubmit)}>
        <Stack style={{ width: 500 }} spacing={2}>
          <DebounceSearch
            inputName='personId'
            control={control}
            setName={setAddedCrewMemberName}
            searchData={{ key: 'people', searchFetch: fetchPeople }}
            noOptionsRender={
              <Button
                color='primary'
                variant='contained'
                onClick={() => setOpen(true)}>
                Create Person
              </Button>
            }
          />

          <Controller
            name='role'
            control={control}
            defaultValue={CrewRole.None}
            rules={{ required: 'Role is required' }}
            render={({ field }) => (
              <TextField
                {...field}
                select
                label='Role'
                fullWidth
                defaultValue={''}>
                {crewRoles?.map((role) => (
                  <MenuItem key={role} value={role}>
                    {CrewRole[role]}
                  </MenuItem>
                ))}
              </TextField>
            )}
          />
          <Button type='submit' variant='contained'>
            Add Crew Member
          </Button>
        </Stack>
      </form>
    </>
  );
}
