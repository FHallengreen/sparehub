import React, { useState, useEffect } from 'react';
import { Modal, Box, Button, TextField } from '@mui/material';
import {supportedTables} from "../../helpers/FrontendDatabase.tsx";

interface EditModalProps {
    open: boolean;
    object: any;
    onClose: () => void;
    onSave: (updatedObject: any) => void;
}

const EditModal: React.FC<EditModalProps> = ({ open, object, onClose, onSave }) => {
    const [formData, setFormData] = useState<any>(object);

    useEffect(() => {
        if (object) {
            setFormData(object); // Ensure all keys are present
        }
    }, [object]);

    // Handle changes in form input
    const handleInputChange = (key: string, value: string) => {
        setFormData((prev: any) => ({
            ...prev,
            [key]: value, // Only update the specific key
        }));
    };

    const handleSave = () => {
        const updatedObject = appendIdToForeignKeys(formData);
        onSave(updatedObject); // Pass the updated object
    };

    const appendIdToForeignKeys = (data: any) => {
        const updatedData: any = {};

        // Loop through the formData and append "Id" to foreign key fields
        Object.entries(data).forEach(([key, value]) => {
            if (value && typeof value === 'object' && value.id) {
                // If it's a foreign key, append "Id" to the key name
                updatedData[`${key}Id`] = value.id; // Add the id and rename the field
            } else {
                updatedData[key] = value; // Otherwise, keep the field as it is
            }
        });

        return updatedData;
    };

    // Function to display only the id for foreign key objects
    const renderFieldValue = (key: string, value: any) => {
        // Check if the value is a foreign key object (it should be an object and have an id property)
        if (value && typeof value === 'object' && value.id) {
            return value.id; // Display only the ID for foreign key objects
        }
        return value; // Otherwise, return the value as is
    };

    if (!open || !formData) return null;

    return (
        <Modal open={open} onClose={onClose}>
            <Box
                sx={{
                    position: 'absolute',
                    top: '50%',
                    left: '50%',
                    transform: 'translate(-50%, -50%)',
                    width: 400,
                    bgcolor: 'background.paper',
                    boxShadow: 24,
                    p: 4,
                }}
            >
                {Object.entries(formData).map(([key, value]) => (
                    <div key={key}>
                        <TextField
                            fullWidth
                            margin="normal"
                            label={key}
                            value={renderFieldValue(key, value)} // Use the renderFieldValue to display id for foreign keys
                            onChange={(e) => handleInputChange(key, e.target.value)}
                            disabled={supportedTables.includes(key)} // ID is read-only
                        />
                    </div>
                ))}
                <div style={{ marginTop: "16px", textAlign: "right" }}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={handleSave}
                        style={{ marginRight: "8px" }}
                    >
                        Save
                    </Button>
                    <Button variant="outlined" color="secondary" onClick={onClose}>
                        Cancel
                    </Button>
                </div>
            </Box>
        </Modal>
    );
};

export default EditModal;
