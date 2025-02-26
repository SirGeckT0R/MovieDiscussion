import { useQuery } from '@tanstack/react-query';
import { getDiscussionsQuery } from '../../queries/discussionsQueries';
import { Grid2 } from '@mui/material';
import { NavLink } from 'react-router-dom';

export function DiscussionPage() {
  const { data: discussions } = useQuery(getDiscussionsQuery());
  return (
    <Grid2>
      {discussions?.map((discussion) => (
        <NavLink to={discussion.id} key={discussion.id}>
          {discussion.title}
        </NavLink>
      ))}
    </Grid2>
  );
}
