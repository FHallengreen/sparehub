import React from 'react';
import { Autocomplete, TextField, Chip, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface PortFilterProps {
    suggestions: string[];
    searchTags: string[];
    setSearchTags: (tags: string[]) => void;
}

const PortFilter: React.FC<PortFilterProps> = ({
    suggestions,
    searchTags,
    setSearchTags,
}) => {
    const navigate = useNavigate();
    return (
        <div className="flex items-center space-x-2">
            <Autocomplete
                multiple
                freeSolo
                options={suggestions}
                value={searchTags}
                onChange={(_, newValue) => setSearchTags(newValue)}
                renderTags={(value, getTagProps) =>
                    value.map((option, index) => (
                        <Chip label={option} {...getTagProps({ index })} />
                    ))
                }
                renderInput={(params) => (
                    <TextField
                        {...params}
                        variant="outlined"
                        label="Search Ports"
                        placeholder="Add a tag"
                        fullWidth
                    />
                )}
                style={{ width: '40vw' }} // Adjust width as needed
            />
            <Button
                onClick={() => navigate(`/ports/new`)}
                variant="contained"
                color="primary"
                 style={{ marginLeft: '10px' }}
                >
                Create New Port
            </Button>
        </div>
    );
};

export default PortFilter;