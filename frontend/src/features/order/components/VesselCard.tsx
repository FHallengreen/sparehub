import React, { useState, useEffect } from 'react';
import { TextField, Typography } from '@mui/material';
import { Vessel } from '../../../interfaces/order'
import { useDebounce } from '../../../hooks/useDebounce';

interface VesselCardProps {
  vessel: Vessel;
  fetchVesselOptions: (query: string) => Promise<Vessel[]>;
  onVesselChange: (vessel: Vessel) => void;
}

const VesselCard: React.FC<VesselCardProps> = ({ vessel, fetchVesselOptions, onVesselChange }) => {
  const [vesselQuery, setvesselQuery] = useState(vessel.name);
  const [localOptions, setLocalOptions] = useState<Vessel[]>([]);
  const debouncedVesselQuery = useDebounce(vesselQuery, 500)

  useEffect(() => {
    const fetchOptions = async () => {
      if (vesselQuery.length >= 3 && vesselQuery !== vessel.name) {
        fetchVesselOptions(debouncedVesselQuery)
          .then(setLocalOptions)
          .catch((err) => {
            console.error('Failed to fetch vessel options:', err);
            setLocalOptions([]);
          });
      } else {
        setLocalOptions([]);
      }
    };
    fetchOptions();
  }, [debouncedVesselQuery, fetchVesselOptions]);


  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setvesselQuery(value);
  };

  const handleSelectVessel = (selected: Vessel) => {
    setvesselQuery(selected.name);
    setLocalOptions([]);
    onVesselChange({
      id: selected.id,
      name: selected.name,
      owner: selected.owner,
    });
  };


  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">ORDER FOR:</Typography>
      <TextField
        label="Vessel Name"
        value={vesselQuery}
        onChange={handleInputChange}
        className="w-full"
        autoComplete="off"
      />
      {vesselQuery.length > 0 && vesselQuery !== vessel.name && localOptions.length > 0 && (
        <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-2 z-50 overflow-y-auto w-64 max-h-40">
          {localOptions.map((option) => (
            <button
              key={option.id}
              onClick={() => handleSelectVessel(option)}
              className="px-4 py-2 cursor-pointer hover:bg-blue-100 text-left w-full"
            >
              {option.name}
            </button>
          ))}
        </div>
      )}

      <TextField
        label="Owner Name"
        value={vessel.owner?.name || ''}
        className="w-full mt-2"
        disabled
      />
    </div>
  );
};

export default VesselCard;
