import { Button, Chip, IconButton, Stack, Typography } from '@mui/material';
import { CrewMember, CrewRole } from '../../../types/movie';
import { Dispatch, SetStateAction, useState } from 'react';
import { Delete, RemoveCircle } from '@mui/icons-material';

const groupOnRole = (crew: CrewMember[]): Array<CrewMember[]> =>
  Object.values(
    crew
      ?.sort((x) => x.role)
      ?.reduce((r, o) => {
        (r[o.role] = r[o.role] || []).push(o);
        return r;
      }, Object.create(null))
  );

export function EditCrewMembers({
  crew,
  setCrew,
}: {
  crew: CrewMember[];
  setCrew: Dispatch<SetStateAction<CrewMember[]>>;
}) {
  const [grouped, setGrouped] = useState<Array<CrewMember[]> | null>(
    groupOnRole(crew)
  );

  const handleDelete = (member: CrewMember) => {
    const index = crew.indexOf(member);
    if (index > -1) {
      crew.splice(index, 1);
      console.log(crew);
    }

    setCrew(crew);
    setGrouped(crew ? groupOnRole(crew) : null);
  };

  return (
    <Stack spacing={2}>
      {crew
        ? grouped?.map((group, role) => (
            <Stack spacing={1} direction={'row'} key={role}>
              <Chip
                label={CrewRole[role + 1]}
                color='secondary'
                sx={{
                  fontWeight: 'bold',
                }}
              />
              {group?.map((member) => (
                <Typography variant='h5' key={member.personId}>
                  {member.fullName}
                  <IconButton onClick={() => handleDelete(member)}>
                    <RemoveCircle />
                  </IconButton>
                </Typography>
              ))}
            </Stack>
          ))
        : null}
    </Stack>
  );
}
