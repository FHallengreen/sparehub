import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import Autocomplete from '@mui/material/Autocomplete';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import DispatchDetailForm from '../components/DispatchDetailForm.tsx';
import { createDispatch, fetchDestinations } from '../../../api/dispatchApi.ts';
import {DispatchRequest} from '../../../interfaces/dispatch.ts';
import { Supplier } from "../../../interfaces/supplier.ts";
import { Vessel } from "../../../interfaces/vessel.ts";
import DispatchOrders from "../components/DispatchOrders.tsx";

const destinationTypeOptions = ['Warehouse', 'Vessel', 'Supplier', 'Address'];
const statusOptions = ['Created', 'Sent', 'Delivered'];

const NewDispatchPage: React.FC = () => {
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();
  const location = useLocation();
  const { selectedData } = location.state || {};
  const userData = JSON.parse(localStorage.getItem('session') || '{}');

  const [dispatch, setDispatch] = useState<DispatchRequest>({
    destinationType: null,
    destinationId: null,
    transportModeType: 'Courier',
    userId: userData.user.id,
    status: 'Created',
    orderIds: selectedData ? selectedData.map((order: any) => order.id.toString()) : [],
  });

  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [destinations, setDestinations] = useState<Supplier[] | Vessel[]>([]);

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
    if (!dispatch.destinationId) {
      setError('Please select a valid origin and destination.');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const sanitizedDispatch: DispatchRequest = {
        ...dispatch,
        destinationId: Number(dispatch.destinationId),
        orderIds: dispatch.orderIds.map((id) => (id)),
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

      {selectedData && (
        <DispatchOrders orders={selectedData} />
      )}

      <DispatchDetailForm
        title="Create Dispatch"
        fields={[
          {
            label: 'Destination Type',
            value: dispatch.destinationType,
            onChange: (value) => handleInputChange('destinationType', value),
            customComponent: (
              <Autocomplete
                options={destinationTypeOptions}
                value={dispatch.destinationType}
                onChange={(_, newValue) => handleInputChange('destinationType', newValue)}
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
                onChange={(_, newValue) => handleInputChange('destinationId', newValue?.id || 0)}
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
            customComponent: (
              <Autocomplete options={['Courier', 'Air', 'Sea', 'Land']}
                value={dispatch.transportModeType}
                onChange={(_, newValue) => handleInputChange('transportModeType', newValue)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Transport Mode"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
          },
          {
            label: 'Status',
            value: dispatch.status,
            onChange: (value) => handleInputChange('status', value),
            customComponent: (
              <Autocomplete
                options={statusOptions}
                value={dispatch.status}
                onChange={(_, newValue) => handleInputChange('status', newValue)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Status"
                    variant="outlined"
                    fullWidth
                  />
                )}
              />
            ),
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
