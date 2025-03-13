import { Chip, IconButton, Stack, Typography } from '@mui/material';
import { CrewMember, CrewRole } from '../../../types/movie';
import { Dispatch, SetStateAction } from 'react';
import { RemoveCircle } from '@mui/icons-material';
import { groupOnRole } from '../../../helpers/crewMemberHelper';

type Props = {
  crew: CrewMember[];
  setCrew: Dispatch<SetStateAction<CrewMember[]>>;
};

export function EditCrewMembers({ crew, setCrew }: Props) {
  const grouped = groupOnRole(crew);

  const handleDelete = (member: CrewMember) => {
    const index = crew.findIndex(
      ({ personId }) => personId === member.personId
    );

    if (index > -1) {
      setCrew((crew) => [...crew.slice(0, index), ...crew.slice(index + 1)]);
    }
  };

  return (
    <Stack spacing={2}>
      {crew
        ? grouped?.map((group) => (
            <Stack spacing={1} direction={'row'} key={group[0].role}>
              <Chip
                label={CrewRole[group[0].role]}
                color='primary'
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
