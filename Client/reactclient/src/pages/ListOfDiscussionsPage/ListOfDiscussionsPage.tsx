import { useQuery } from '@tanstack/react-query';
import { getListOfDiscussionsQuery } from '../../queries/discussionsQueries';
import { Button, Grid2, Stack } from '@mui/material';
import { DiscussionCard } from './components/DiscussionCard';
import { CreateDiscussionForm } from './components/CreateDiscussionForm';
import { useState } from 'react';
import { queryClient } from '../../api/global';

export function ListOfDiscussionsPage() {
  const [isCreateMode, setIsCreateMode] = useState<boolean>(false);

  const discussionsQuery = getListOfDiscussionsQuery();
  const { data: discussions } = useQuery(discussionsQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(discussionsQuery.queryKey);
  };

  return (
    <Stack spacing={3}>
      <Grid2 container spacing={2} columns={3}>
        {discussions?.map((discussion) => (
          <DiscussionCard discussion={discussion} key={discussion.id} />
        ))}
      </Grid2>
      {isCreateMode ? (
        <CreateDiscussionForm
          queryInvalidator={queryInvalidator}
          createMode={setIsCreateMode}
        />
      ) : (
        <Button variant='contained' onClick={() => setIsCreateMode(true)}>
          Create new discussion
        </Button>
      )}
    </Stack>
  );
}
