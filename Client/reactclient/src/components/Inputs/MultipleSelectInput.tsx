import { MenuItem, Select } from '@mui/material';
import { Control, Controller } from 'react-hook-form';
import { MovieGenresView } from '../MovieGenresView';

export function CustomSelectInput({
  options,
  control,
  inputName,
  isMultiple,
  isRequired = true,
}: {
  options?: Array<{ id: string; name: string }>;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<any, any>;
  inputName: string;
  isMultiple: boolean;
  isRequired?: boolean;
}) {
  return (
    <Controller
      control={control}
      name={inputName}
      defaultValue={[]}
      rules={{ required: isRequired }}
      render={({ field }) => {
        return (
          <Select
            {...field}
            multiple={isMultiple}
            MenuProps={{
              PaperProps: {
                style: {
                  maxHeight: 300,
                  overflow: 'auto',
                },
              },
            }}
            renderValue={(selected: string[]) => {
              return (
                <MovieGenresView
                  genres={options?.filter((option) =>
                    selected.includes(option.id)
                  )}
                />
              );
            }}
            label={inputName.charAt(0).toUpperCase() + inputName.slice(1)}
            required={isRequired}>
            <MenuItem value={''} hidden disabled />
            {options?.map((option) => (
              <MenuItem
                value={option.id}
                key={option.id}
                sx={{
                  '&.Mui-selected': {
                    backgroundColor: '#90AEAD',
                    fontWeight: 'bold',
                  },
                }}>
                {option.name}
              </MenuItem>
            ))}
          </Select>
        );
      }}
    />
  );
}
