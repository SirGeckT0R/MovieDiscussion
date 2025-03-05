import { Button, Stack, Typography } from '@mui/material';
import { NavLink, useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { getDiscussionQuery } from '../../queries/discussionsQueries';
import { SubscribeDiscussionAction } from './components/SubscribeDiscussionAction';
import { DeleteDiscussionAction } from './components/DeleteDiscussionAction';
import { useAuth } from '../../hooks/useAuth';

export function DiscussionPage() {
  const { id } = useParams();
  const { user } = useAuth();
  const { data: discussion } = useQuery(getDiscussionQuery(id!));

  return (
    <Stack spacing={2} alignItems={'center'}>
      <Typography variant='h3'>{discussion?.title}</Typography>
      <Typography variant='h4'>{discussion?.description}</Typography>
      <Typography variant='h4' color='inherit'>
        Starting at {discussion?.startAt}
      </Typography>
      <Typography variant='h4' color='inherit'>
        {discussion?.isActive ? 'Active' : 'Not active'}
      </Typography>
      <NavLink to='edit'>
        <Button color='primary' variant='contained'>
          Edit
        </Button>
      </NavLink>
      <NavLink to='chat'>
        <Button color='primary' variant='contained'>
          Go to Chat
        </Button>
      </NavLink>
      <DeleteDiscussionAction id={id} />
      {!discussion?.subscribers.includes(user.id ?? '') && (
        <SubscribeDiscussionAction id={id} />
      )}
    </Stack>
  );
}
