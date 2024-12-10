import React from 'react';
import { Autocomplete, TextField, Chip, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface OwnerFilterProps {
  suggestions: string[];
  searchTags: string[];
  setSearchTags: (tags: string[]) => void;
  searchBoxRef: React.RefObject<HTMLInputElement>;
}

const OwnerFilter: React.FC<OwnerFilterProps> = ({
  suggestions,
  searchTags,
  setSearchTags,
  searchBoxRef,
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
          value.map((option, index) => {
            const { key, ...tagProps } = getTagProps({ index });
            return <Chip key={key} variant="outlined" label={option} {...tagProps} />;
          })
        }
        renderInput={(params) => (
          <TextField
            {...params}
            inputRef={searchBoxRef}
            variant="outlined"
            label="Search Owners"
            placeholder="Add a tag"
          />
        )}
        style={{ width: '40vw' }}
      />
      <Button
        onClick={() => navigate(`/owners/new`)}
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
