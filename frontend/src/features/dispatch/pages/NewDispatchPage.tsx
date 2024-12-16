import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import Autocomplete from '@mui/material/Autocomplete';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import DispatchDetailForm from '../components/DispatchDetailForm.tsx';
import { createDispatch, fetchDestinations, fetchOrigins } from '../../../api/dispatchApi.ts';
import { DispatchRequest } from '../../../interfaces/dispatch.ts';
import { Supplier } from "../../../interfaces/supplier.ts";
import { Warehouse } from "../../../interfaces/warehouse.ts";
import { Vessel } from "../../../interfaces/vessel.ts";

const originTypeOptions = ['Warehouse', 'Supplier'];
const destinationTypeOptions = ['Warehouse', 'Vessel'];

const NewDispatchPage: React.FC = () => {
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();
  const location = useLocation();
  const { selectedData } = location.state || {};

  const [dispatch, setDispatch] = useState<DispatchRequest>({
    originType: '',
    originId: 0,
    destinationType: '',
    destinationId: null,
    transportModeType: '',
    userId: 0,
    orderIds: selectedData ? selectedData.map((order: any) => order.id.toString()) : [],
  });

  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [origins, setOrigins] = useState<Supplier[] | Warehouse[]>([]);
  const [destinations, setDestinations] = useState<Supplier[] | Vessel[]>([]);

  // Fetch Origins
  useEffect(() => {
    const loadOrigins = async () => {
      if (dispatch.originType) {
        const fetchedOrigins = await fetchOrigins(dispatch.originType);
        setOrigins(fetchedOrigins);
      } else {
        setOrigins([]);
      }
    };
    loadOrigins();
  }, [dispatch.originType]);

  // Fetch Destinations
  useEffect(() => {
    const loadDestinations = async () => {
      if (dispatch.destinationType) {
        const fetchedDestinations = await fetchDestinations(dispatch.destinationType);
        setDestinations(fetchedDestinations);
      } else {
        setDestinations([]);
      }
    };
    loadDestinations();
  }, [dispatch.destinationType]);

  const handleInputChange = (field: keyof DispatchRequest, value: string | number | null) => {
    setDispatch((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSave = async () => {
    if (!dispatch.originId || !dispatch.destinationId) {
      setError('Please select a valid origin and destination.');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      // Ensure numeric types for IDs
      const sanitizedDispatch: DispatchRequest = {
        ...dispatch,
        originId: Number(dispatch.originId),
        destinationId: Number(dispatch.destinationId),
        orderIds: dispatch.orderIds.map((id) => Number(id)),
      };

      await createDispatch(sanitizedDispatch);
      showSnackbar('Dispatch created successfully!', 'success');
      navigate('/dispatches');
    } catch (err) {
      console.error('Error creating dispatch:', err);
      showSnackbar('Failed to create dispatch.', 'error');
      setError('Failed to create dispatch.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  return (
    <div className="container mx-auto p-6">
      <Typography variant="h4" className="text-2xl font-bold mb-6">
        Create New Dispatch
      </Typography>

      {error && <Typography color="error">{error}</Typography>}

      <DispatchDetailForm
        title="Create Dispatch"
        fields={[
          {
            label: 'Origin Type',
            value: dispatch.originType,
            onChange: (value) => handleInputChange('originType', value),
            customComponent: (
              <Autocomplete
                options={originTypeOptions}
                value={dispatch.originType}
                onChange={(event, newValue) => handleInputChange('originType', newValue)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Origin Type"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
          },
          {
            label: 'Origin',
            value: dispatch.originId,
            onChange: (value) => handleInputChange('originId', value),
            customComponent: (
              <Autocomplete
                options={origins}
                getOptionLabel={(option) => option.name}
                onChange={(event, newValue) => handleInputChange('originId', newValue?.id || 0)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Origin"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
          },
          {
            label: 'Destination Type',
            value: dispatch.destinationType,
            onChange: (value) => handleInputChange('destinationType', value),
            customComponent: (
              <Autocomplete
                options={destinationTypeOptions}
                value={dispatch.destinationType}
                onChange={(event, newValue) => handleInputChange('destinationType', newValue)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Destination Type"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
          },
          {
            label: 'Destination',
            value: dispatch.destinationId,
            onChange: (value) => handleInputChange('destinationId', value),
            customComponent: (
              <Autocomplete
                options={destinations}
                getOptionLabel={(option) => option.name}
                onChange={(event, newValue) => handleInputChange('destinationId', newValue?.id || 0)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Destination"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
          },
          {
            label: 'Transport Mode',
            value: dispatch.transportModeType,
            onChange: (value) => handleInputChange('transportModeType', value),
          },
        ]}
      />

      <div className="mt-8 gap-2 flex">
        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
          Save
        </Button>
        <Button onClick={() => navigate('/dispatches')} variant="outlined">
          Back
        </Button>
      </div>
    </div>
  );
};

export default NewDispatchPage;
