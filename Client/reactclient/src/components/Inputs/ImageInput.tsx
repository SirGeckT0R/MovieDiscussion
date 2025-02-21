import { ImageSharp } from '@mui/icons-material';
import { Box, InputLabel, Typography } from '@mui/material';
import { Dispatch, SetStateAction } from 'react';

export function ImageInput({
  imageState,
}: {
  imageState: {
    image: Blob | null;
    setImage: Dispatch<SetStateAction<Blob | null>>;
  };
}) {
  return (
    <Box alignSelf={'center'}>
      <InputLabel
        htmlFor='image'
        sx={(theme) => ({
          border: 2,
          borderRadius: 2,
          borderColor: theme.palette.primary.light,
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          cursor: 'pointer',
          width: 300,
          height: 300,
          background: theme.palette.grey[200],
        })}>
        {imageState.image ? (
          <img
            src={URL.createObjectURL(imageState.image)}
            style={{
              width: '100%',
              height: '100%',
              objectFit: 'cover',
            }}
          />
        ) : (
          <>
            <ImageSharp sx={{ width: 100, height: 100 }} />
            <Typography>Add Image</Typography>
          </>
        )}
      </InputLabel>
      <input
        accept='image/*'
        id='image'
        type='file'
        style={{ display: 'none' }}
        onChange={(event) => {
          if (!event.target.files) {
            imageState.setImage(null);
            return;
          }
          imageState.setImage(event.target.files[0]);
        }}
      />
    </Box>
  );
}
