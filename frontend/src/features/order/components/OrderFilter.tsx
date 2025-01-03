import React from 'react';
import { Autocomplete, TextField, Chip } from '@mui/material';

interface OrderFilterProps {
  suggestions: string[];
  searchTags: string[];
  setSearchTags: (tags: string[]) => void;
}

const OrderFilter: React.FC<OrderFilterProps> = ({ suggestions, searchTags, setSearchTags }) => {
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
          <TextField {...params} label="Search Orders" placeholder="Add a tag" />
        )}
        style={{ width: '40vw' }}
      />
    </div>
  );
};

export default OrderFilter;
