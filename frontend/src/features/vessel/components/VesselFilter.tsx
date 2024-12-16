import React from 'react';
import { Autocomplete, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface VesselFilterProps {
    suggestions: string[];
    searchTerm: string;
    setSearchTerm: (term: string) => void;
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
                style={{ width: '40vw' }}
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