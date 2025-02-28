import { Button, MenuItem, Stack, TextField } from '@mui/material';
import { Dispatch, SetStateAction, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { CrewMember, CrewRole, crewRoles } from '../../../types/movie';
import { fetchPeople } from '../../../api/peopleService';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';
import { CreatePersonInput } from './CreatePersonInput';

export function CrewMemberInputForm({
  crewState,
}: {
  crewState: {
    crew: CrewMember[];
    setCrew: Dispatch<SetStateAction<CrewMember[]>>;
  };
}) {
  const [open, setOpen] = useState(false);
  const [addedCrewMemberName, setAddedCrewMemberName] = useState('');

  const { handleSubmit: handleCrewSubmit, control: crewControl } =
    useForm<CrewMember>();

  const onCrewSubmit = (formBody: CrewMember) => {
    formBody.fullName = addedCrewMemberName;
    crewState.setCrew(crewState.crew.concat(formBody));
  };

  return (
    <>
      <CreatePersonInput openState={{ open, setOpen }} />
      <form onSubmit={handleCrewSubmit(onCrewSubmit)}>
        <Stack style={{ width: 500 }} spacing={2}>
          <DebounceSearch
            inputName='personId'
            control={crewControl}
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
            control={crewControl}
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
