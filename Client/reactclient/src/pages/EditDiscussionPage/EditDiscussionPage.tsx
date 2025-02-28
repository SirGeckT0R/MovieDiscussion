import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate, useParams } from 'react-router-dom';
import { getDiscussionQuery } from '../../queries/discussionsQueries';
import { Button, Stack, TextField, Typography } from '@mui/material';
import { UpdateDiscussionRequest } from '../../types/discussion';
import { updateDiscussion } from '../../api/discussionsService';
import { useForm } from 'react-hook-form';

export function EditDiscussionPage() {
  const { id } = useParams();
  const { data: discussion } = useQuery(getDiscussionQuery(id!));

  const navigate = useNavigate();
  const { register: update, handleSubmit: handleUpdateDiscussion } =
    useForm<UpdateDiscussionRequest>({
      defaultValues: {
        id: discussion?.id,
        description: discussion?.description,
        title: discussion?.title,
      },
    });

  const { mutateAsync } = useMutation({
    mutationFn: (values: UpdateDiscussionRequest) => updateDiscussion(values),
  });

  const onUpdateDiscussion = (values: UpdateDiscussionRequest) => {
    mutateAsync(values).then(() => navigate(`/discussions/${id}`));
  };

  return (
    <form onSubmit={handleUpdateDiscussion(onUpdateDiscussion)}>
      <Stack spacing={2}>
        <Typography variant='h3'>Edit a discussion</Typography>
        <TextField
          type='text'
          id='titleInput'
          label='Title'
          {...update('title')}
          required
        />
        <TextField
          type='text'
          id='descriptionInput'
          label='Description'
          {...update('description')}
          required
        />
        <Button type='submit' variant='contained'>
          Update
        </Button>
      </Stack>
    </form>
  );
}
