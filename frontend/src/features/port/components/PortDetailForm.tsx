import React from "react";
import { TextField, Typography } from "@mui/material";

interface Field {
  label: string;
  value: string | number | null;
  onChange: (value: any) => void;
}

interface PortDetailFormProps {
  title: string;
  fields: Field[];
}

const PortDetailForm: React.FC<PortDetailFormProps> = ({ title, fields }) => {
  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">
        {title}
      </Typography>
      <div className="flex flex-col gap-4">
        {fields.map((field) => (
          <TextField
            key={field.label}
            label={field.label}
            value={field.value ?? ""}
            onChange={(e) => field.onChange(e.target.value)}
            className="w-full"
          />
        ))}
      </div>
    </div>
  );
};

export default PortDetailForm;