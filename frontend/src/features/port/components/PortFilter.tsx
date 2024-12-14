import React from 'react';
import { Autocomplete, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface PortFilterProps {
    suggestions: string[];
    searchTerm: string;
    setSearchTerm: (term: string) => void;

}

const PortFilter: React.FC<PortFilterProps> = ({
    suggestions,
    searchTerm,
    setSearchTerm,
}) => {
    const navigate = useNavigate();
    
    return (
        <div className="flex items-center space-x-2">
            <Autocomplete
                freeSolo
                options={suggestions}
                inputValue={searchTerm}
                onInputChange={(_, newValue) => setSearchTerm(newValue)}
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
