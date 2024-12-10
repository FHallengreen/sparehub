import React from 'react';
import { Autocomplete, TextField, Chip, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface VesselFilterProps {
    suggestions: string[];
    searchTags: string[];
    setSearchTags: (tags: string[]) => void;
}

const VesselFilter: React.FC<VesselFilterProps> = ({
    suggestions,
    searchTags,
    setSearchTags,
}) => {
    const navigate = useNavigate();
    return (
        <div className="flex items-center space-x-2 mb-5">
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
                        label="Search Vessels"
                        placeholder="Add a tag"
                        fullWidth
                    />
                )}
                style={{ width: '40vw' }}
            />
          <Button
        onClick={() => navigate(`/vessels/new`)}
        variant="contained"
        color="primary"
        style={{ marginLeft: '10px' }}
             >
                Create New Vessel
            </Button>
        </div>
    );
};

export default VesselFilter; 