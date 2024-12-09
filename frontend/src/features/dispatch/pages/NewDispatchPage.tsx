import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import DispatchDetailForm from '../components/DispatchDetailForm.tsx';
import { createDispatch } from '../../../api/dispatchApi.ts';
import { DispatchRequest } from '../../../interfaces/dispatch.ts';

const NewDispatchPage: React.FC = () => {
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();

  const [dispatch, setDispatch] = useState<DispatchRequest>({
    originType: '',
    originId: 0,
    destinationType: '',
    destinationId: null,
    dispatchStatus: '',
    transportModeType: '',
    trackingNumber: '',
    dispatchDate: null,
    deliveryDate: null,
    userId: 0,
  });

  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const handleInputChange = (field: keyof DispatchRequest, value: string | number | null) => {
    setDispatch((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSave = async () => {
    setLoading(true);
    setError(null);

    try {
      await createDispatch(dispatch);
      showSnackbar('Dispatch created successfully!', 'success');
      navigate('/dispatches');
    } catch (err) {
      console.error('Error creating dispatch:', err); // Log the error
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
          { label: 'Origin Type', value: dispatch.originType, onChange: (value) => handleInputChange('originType', value) },
          { label: 'Origin ID', value: dispatch.originId, onChange: (value) => handleInputChange('originId', value), type: 'number' },
          { label: 'Destination Type', value: dispatch.destinationType, onChange: (value) => handleInputChange('destinationType', value) },
          { label: 'Destination ID', value: dispatch.destinationId, onChange: (value) => handleInputChange('destinationId', value) },
          { label: 'Status', value: dispatch.dispatchStatus, onChange: (value) => handleInputChange('dispatchStatus', value) },
          { label: 'Transport Mode', value: dispatch.transportModeType, onChange: (value) => handleInputChange('transportModeType', value) },
          { label: 'Tracking Number', value: dispatch.trackingNumber, onChange: (value) => handleInputChange('trackingNumber', value) },
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
