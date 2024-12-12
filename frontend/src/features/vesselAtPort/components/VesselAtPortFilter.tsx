import React from 'react';
import { Autocomplete, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface VesselAtPortFilterProps {
    suggestions: string[];
    searchTerm: string;
    setSearchTerm: (term: string) => void;
}

const VesselAtPortFilter: React.FC<VesselAtPortFilterProps> = ({
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
                getOptionLabel={(option) => option || ''} // Handle undefined values
                renderInput={(params) => (
                    <TextField
                        {...params}
                        variant="outlined"
                        label="Search Ports"
                        placeholder="Type to search"
                        fullWidth
                    />
                )}
                style={{ width: '40vw' }} // Adjust width as needed
            />
            <Button
                onClick={() => navigate('/vessels-at-ports/new')}
                variant="contained"
                color="primary"
                style={{ marginLeft: '10px' }}
            >
                Create New Vessel at Port
            </Button>
        </div>
    );
};

export default VesselAtPortFilter;