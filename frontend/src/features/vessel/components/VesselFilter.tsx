import React from 'react';
import { Autocomplete, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface VesselFilterProps {
    suggestions: string[]; // Suggestions for autocomplete
    searchTerm: string; // Current search term
    setSearchTerm: (term: string) => void; // Function to update search term
}

const VesselFilter: React.FC<VesselFilterProps> = ({
    suggestions,
    searchTerm,
    setSearchTerm,
}) => {
    const navigate = useNavigate();

    return (
        <div className="flex items-center space-x-2 mb-4">
            <Autocomplete
                freeSolo
                options={suggestions}
                inputValue={searchTerm}
                onInputChange={(_, newValue) => setSearchTerm(newValue)}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        variant="outlined"
                        label="Search Vessels"
                        placeholder="Type to search"
                        fullWidth
                    />
                )}
                style={{ width: '40vw' }} // Adjust width as needed
            />
            <Button
                onClick={() => navigate('/vessels/new')}
                variant="contained"
                color="primary"
                style={{ marginLeft: '10px' }}
            >
                Create Vessel
            </Button>
        </div>
    );
};

export default VesselFilter;