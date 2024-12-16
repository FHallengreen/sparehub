import React from "react";
import { TextField, Typography } from "@mui/material";

interface Field {
  label: string;
  value: string | number | null;
  onChange: (value: any) => void;
  select?: boolean;
  options?: { value: string; label: string }[];
}

interface VesselDetailFormProps {
  title: string;
  fields: Field[];
}

const VesselDetailForm: React.FC<VesselDetailFormProps> = ({ title, fields }) => {
  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">
        {title}
      </Typography>
      <div className="flex flex-col gap-4">
        {fields.map((field) =>
          field.select ? (
            <TextField
              key={field.label}
              select
              label={field.label}
              value={field.value ?? ""}
              onChange={(e) => field.onChange(e.target.value)}
              fullWidth
              className="mb-4"
              SelectProps={{
                native: true,
              }}
              InputLabelProps={{
                shrink: true,
              }}
            >
              {field.options?.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </TextField>
          ) : (
            <TextField
              key={field.label}
              label={field.label}
              value={field.value ?? ""}
              onChange={(e) => field.onChange(e.target.value)}
              className="w-full"
            />
          )
        )}
      </div>
    </div>
  );
};

export default VesselDetailForm;