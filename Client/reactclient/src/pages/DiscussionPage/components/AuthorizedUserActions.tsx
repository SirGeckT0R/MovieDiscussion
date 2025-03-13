import { Button } from '@mui/material';
import { NavLink } from 'react-router-dom';
import { DeleteDiscussionAction } from './DeleteDiscussionAction';
import { SubscribeDiscussionAction } from './SubscribeDiscussionAction';
import { Discussion } from '../../../types/discussion';
import { useAuth } from '../../../hooks/useAuth';

type Props = {
  discussion: Discussion | undefined;
  queryInvalidator: () => void;
};

export function AuthorizedUserActions({ discussion, queryInvalidator }: Props) {
  const { user } = useAuth();

  return (
    <>
      {discussion?.createdBy === user.id && (
        <>
          <NavLink to='edit'>
            <Button color='primary' variant='contained'>
              Edit
            </Button>
          </NavLink>
          <DeleteDiscussionAction id={discussion.id} />
        </>
      )}
      {discussion?.isActive && (
        <NavLink to='chat'>
          <Button color='primary' variant='contained'>
            Go to Chat
          </Button>
        </NavLink>
      )}
      {!discussion?.subscribers.includes(user.id ?? '') && (
        <SubscribeDiscussionAction
          id={discussion?.id}
          queryInvalidator={queryInvalidator}
        />
      )}
    </>
  );
}
