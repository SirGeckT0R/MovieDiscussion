import { Button, Stack, Typography } from '@mui/material';
import { NavLink, useNavigate, useParams } from 'react-router-dom';
import { useMutation, useQuery } from '@tanstack/react-query';
import { getDiscussionQuery } from '../../queries/discussionsQueries';
import {
  deleteDiscussion,
  subscribeToDiscussion,
} from '../../api/discussionsService';

export function DiscussionPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { data: discussion } = useQuery(getDiscussionQuery(id!));

  const { mutateAsync: deleteDiscussionAsync } = useMutation({
    mutationFn: (id: string) => deleteDiscussion(id),
  });

  const { mutateAsync: subscribeAsync } = useMutation({
    mutationFn: (id: string) => subscribeToDiscussion(id),
  });

  const handleDiscussionDelete = () => {
    deleteDiscussionAsync(id!).then(() => navigate('/discussions'));
  };

  const handleSubcription = () => {
    subscribeAsync(id!);
  };

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
      <Button
        color='error'
        variant='contained'
        onClick={handleDiscussionDelete}>
        Delete
      </Button>
      <Button variant='contained' color='warning' onClick={handleSubcription}>
        Get Notification
      </Button>
    </Stack>
  );
}
