import { Stack, Typography } from '@mui/material';
import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { getDiscussionQuery } from '../../queries/discussionsQueries';
import { useAuth } from '../../hooks/useAuth';
import { queryClient } from '../../api/global';
import { Role } from '../../types/user';
import { AuthorizedUserActions } from './components/AuthorizedUserActions';
import { DateDisplay } from '../../components/MovieFieldsView/DateDisplay';

export function DiscussionPage() {
  const { id } = useParams();
  const { user } = useAuth();

  const discussionQuery = getDiscussionQuery(id);
  const { data: discussion } = useQuery(discussionQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(discussionQuery.queryKey);
  };

  return (
    <Stack spacing={2} alignItems={'center'}>
      <Typography variant='h3'>{discussion?.title}</Typography>
      <Typography variant='h4'>{discussion?.description}</Typography>
      <Typography variant='h4' color='inherit'>
        Starting at{' '}
        <DateDisplay
          date={new Date(discussion?.startAt ?? '')}
          hideTime={true}
        />
      </Typography>
      <Typography variant='h4' color='inherit'>
        {discussion?.isActive ? 'Active' : 'Not active'}
      </Typography>
      {user.role !== Role.Guest && (
        <AuthorizedUserActions
          discussion={discussion}
          queryInvalidator={queryInvalidator}
        />
      )}
    </Stack>
  );
}
