import { MenuItem, Select } from '@mui/material';
import { Control, Controller } from 'react-hook-form';

export function SelectInput({
  options,
  control,
  inputName,
  isMultiple,
}: {
  options?: Array<{ id: string; name: string }>;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<any, any>;
  inputName: string;
  isMultiple: boolean;
}) {
  return (
    <Controller
      control={control}
      name={inputName}
      defaultValue={[]}
      rules={{ required: true }}
      render={({ field }) => {
        return (
          <Select
            {...field}
            multiple={isMultiple}
            fullWidth
            MenuProps={{
              PaperProps: {
                style: {
                  maxHeight: 300,
                  overflow: 'auto',
                },
              },
            }}
            label={inputName.charAt(0).toUpperCase() + inputName.slice(1)}
            required>
            <MenuItem value={''} hidden disabled></MenuItem>
            {options?.map((option) => (
              <MenuItem value={option.id} key={option.id}>
                {option.name}
              </MenuItem>
            ))}
          </Select>
        );
      }}
    />
  );
}
