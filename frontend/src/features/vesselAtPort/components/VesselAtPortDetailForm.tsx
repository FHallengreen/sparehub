import React from "react";
import { TextField, Typography } from "@mui/material";

type Field = {
    label: string;
    value: string;
    onChange: (value: string) => void;
    select?: boolean;
    options?: { value: string; label: string }[];
    type?: string;
    InputLabelProps?: any;
};

interface VesselDetailFormProps {
    title: string;
    fields: Field[];
}

const VesselAtPortDetailForm: React.FC<VesselDetailFormProps> = ({ title, fields }) => {
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
                            <option value="" disabled>Select a {field.label.toLowerCase()}</option>
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
                            type={field.type}
                            fullWidth
                            className="mb-4"
                            InputLabelProps={field.InputLabelProps}
                        />
                    )
                )}
            </div>
        </div>
    );
};

export default VesselAtPortDetailForm;