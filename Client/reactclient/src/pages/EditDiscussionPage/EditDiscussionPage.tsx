import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate, useParams } from 'react-router-dom';
import { getDiscussionQuery } from '../../queries/discussionsQueries';
import { Button, Stack, TextField, Typography } from '@mui/material';
import { UpdateDiscussionRequest } from '../../types/discussion';
import { updateDiscussion } from '../../api/discussionsService';
import { useForm } from 'react-hook-form';

export function EditDiscussionPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const discussionQuery = getDiscussionQuery(id);
  const { data: discussion } = useQuery(discussionQuery);

  const { register, handleSubmit } = useForm<UpdateDiscussionRequest>({
    defaultValues: discussion,
  });

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdateDiscussionRequest) => updateDiscussion(values),
  });

  const onUpdateDiscussion = (values: UpdateDiscussionRequest) => {
    mutateAsync(values).then(() => navigate(`/discussions/${id}`));
  };

  return (
    <form onSubmit={handleSubmit(onUpdateDiscussion)}>
      <Stack spacing={2}>
        <Typography variant='h3'>Edit a discussion</Typography>
        <TextField
          type='text'
          id='titleInput'
          label='Title'
          multiline
          {...register('title')}
          required
        />
        <TextField
          type='text'
          id='descriptionInput'
          label='Description'
          multiline
          {...register('description')}
          required
        />
        <Button type='submit' variant='contained'>
          Update
        </Button>
      </Stack>
    </form>
  );
}
