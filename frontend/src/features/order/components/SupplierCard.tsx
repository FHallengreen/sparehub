import React, { useState, useEffect } from 'react';
import { TextField, Typography } from '@mui/material';
import { Supplier } from '../../../interfaces/order'
import { useDebounce } from '../../../hooks/useDebounce';

interface SupplierCardProps {
  supplier: Supplier;
  supplierOrderNumber: string;
  fetchSupplierOptions: (query: string) => Promise<Supplier[]>;
  onSupplierChange: (supplier: Supplier) => void;
  onOrderNumberChange: (supplierOrderNumber: string) => void;
}

const SupplierCard: React.FC<SupplierCardProps> = ({
  supplier,
  supplierOrderNumber,
  fetchSupplierOptions,
  onSupplierChange,
  onOrderNumberChange,
}) => {
  const [supplierQuery, setsupplierQuery] = useState(supplier.name);
  const [localOptions, setLocalOptions] = useState<Supplier[]>([]);
  const debouncedSupplierQuery = useDebounce(supplierQuery, 500)

  useEffect(() => {
    const fetchOptions = async () => {
      if (supplierQuery.length >= 3 && supplierQuery !== supplier.name) {
        fetchSupplierOptions(debouncedSupplierQuery)
          .then(setLocalOptions)
          .catch((err) => {
            console.error('Failed to fetch supplier options:', err);
            setLocalOptions([]);
          });
      } else {
        setLocalOptions([]);
      }
    };
    fetchOptions();

  }, [debouncedSupplierQuery, fetchSupplierOptions]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setsupplierQuery(value);
  };

  const handleSelectSupplier = (selected: Supplier) => {
    setsupplierQuery(selected.name);
    setLocalOptions([]);
    onSupplierChange(selected);
  };

  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">SUPPLIER:</Typography>
      <TextField
        label="Supplier Name"
        value={supplierQuery}
        onChange={handleInputChange}
        className="w-full"
        autoComplete="off"
      />
      {supplierQuery.length > 0 && supplierQuery !== supplier.name && localOptions.length > 0 && (
        <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-2 z-50 overflow-y-auto w-64 max-h-40">
          {localOptions.map((option) => (
            <button
              key={option.id}
              onClick={() => handleSelectSupplier(option)}
              className="px-4 py-2 cursor-pointer hover:bg-blue-100 text-left w-full"
            >
              {option.name}
            </button>
          ))}
        </div>
      )}
      <TextField
        label="Supplier Order Number"
        value={supplierOrderNumber}
        onChange={(e) => onOrderNumberChange(e.target.value)}
        className="w-full mt-2"
      />
    </div>
  );
};

export default SupplierCard;
