import React from 'react';
import { Autocomplete, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface OwnerFilterProps {
  suggestions: string[];
  searchTerm: string; // Current search term
  setSearchTerm: (term: string) => void; // Function to update search term
}

const OwnerFilter: React.FC<OwnerFilterProps> = ({
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
            label="Search Owners"
            placeholder="Type to search"
            fullWidth
          />
        )}
        style={{ width: '40vw' }} // Adjust width as needed
      />
      <Button
        onClick={() => navigate('/owners/new')}
        variant="contained"
        color="primary"
        style={{ marginLeft: '10px' }}
      >
        Create New Owner
      </Button>
    </div>
  );
};

export default OwnerFilter;
