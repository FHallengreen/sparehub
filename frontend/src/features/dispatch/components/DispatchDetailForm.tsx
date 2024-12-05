import React from 'react';
import { TextField, Typography } from '@mui/material';

interface Field {
  label: string;
  value: string | number | null; // Allow `null` as a valid type
  onChange: (value: any) => void;
  type?: string;
}

interface DispatchDetailFormProps {
  title: string;
  fields: Field[];
}

const DispatchDetailForm: React.FC<DispatchDetailFormProps> = ({ title, fields }) => {
  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">{title}</Typography>
      <div className="flex flex-col gap-4">
        {fields.map((field, index) => (
          <TextField
            key={index}
            label={field.label}
            value={field.value ?? ''} // Use an empty string if value is `null`
            onChange={(e) => field.onChange(field.type === 'number' ? +e.target.value : e.target.value)}
            type={field.type}
            className="w-full"
          />
        ))}
      </div>
    </div>
  );
};

export default DispatchDetailForm;
