import * as React from 'react';
import { useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import axios from 'axios';
import { DispatchRequest } from '../interfaces/dispatch';
import { useSnackbar } from './SnackbarContext';

const API_URL = import.meta.env.VITE_API_URL;

const NewDispatchPage: React.FC = () => {
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();

  const [dispatch, setDispatch] = React.useState<DispatchRequest>({
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

  const [loading, setLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string | null>(null);

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
      const response = await axios.post(`${API_URL}/api/dispatch`, dispatch);
      showSnackbar('Dispatch created successfully!', 'success');
      navigate(`/dispatches/${response.data.id}`);
    } catch (err) {
      showSnackbar('Failed to create dispatch.', 'error');
      setError('Failed to create dispatch.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto p-6">
      {loading && <CircularProgress />}
      {error && <Typography color="error">{error}</Typography>}

      {!loading && !error && (
        <>
          <Typography variant="h4" className="text-2xl font-bold mb-6">
            Create New Dispatch
          </Typography>

          <div className="grid grid-cols-2 gap-6 mb-6">
            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4">ORIGIN DETAILS:</Typography>
              <div className="flex flex-col gap-4">
                <TextField
                  label="Origin Type"
                  value={dispatch.originType}
                  className="w-full"
                  onChange={(e) => handleInputChange('originType', e.target.value)}
                />
                <TextField
                  label="Origin ID"
                  value={dispatch.originId}
                  className="w-full"
                  onChange={(e) => handleInputChange('originId', parseInt(e.target.value))}
                />
              </div>
            </div>

            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4">DESTINATION DETAILS:</Typography>
              <div className="flex flex-col gap-4">
                <TextField
                  label="Destination Type"
                  value={dispatch.destinationType}
                  className="w-full"
                  onChange={(e) => handleInputChange('destinationType', e.target.value)}
                />
                <TextField
                  label="Destination ID"
                  value={dispatch.destinationId || ''}
                  className="w-full"
                  onChange={(e) => handleInputChange('destinationId', e.target.value || null)}
                />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-6 mb-6">
            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4">DISPATCH DETAILS:</Typography>
              <div className="flex flex-col gap-4">
                <TextField
                  label="Status"
                  value={dispatch.dispatchStatus}
                  className="w-full"
                  onChange={(e) => handleInputChange('dispatchStatus', e.target.value)}
                />
                <TextField
                  label="Transport Mode"
                  value={dispatch.transportModeType}
                  className="w-full"
                  onChange={(e) => handleInputChange('transportModeType', e.target.value)}
                />
                <TextField
                  label="Tracking Number"
                  value={dispatch.trackingNumber || ''}
                  className="w-full"
                  onChange={(e) => handleInputChange('trackingNumber', e.target.value)}
                />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-6 mb-6">
            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4">DATES:</Typography>
              <div className="flex flex-col gap-4">
                <TextField
                  label="Dispatch Date"
                  type="date"
                  value={dispatch.dispatchDate ? dispatch.dispatchDate.toISOString().split('T')[0] : ''}
                  InputLabelProps={{ shrink: true }}
                  onChange={(e) =>
                    handleInputChange('dispatchDate', e.target.value ? (new Date(e.target.value) as unknown as string | number | null) : null)
                  }
                  className="w-full"
                />
                <TextField
                  label="Delivery Date"
                  type="date"
                  value={dispatch.deliveryDate ? dispatch.deliveryDate.toISOString().split('T')[0] : ''}
                  InputLabelProps={{ shrink: true }}
                  onChange={(e) =>
                    handleInputChange('dispatchDate', e.target.value ? (new Date(e.target.value) as unknown as string | number | null) : null)
                  }
                  className="w-full"
                />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-6 mb-6">
            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4">USER DETAILS:</Typography>
              <div className="flex flex-col gap-4">
                <TextField
                  label="User ID"
                  value={dispatch.userId}
                  className="w-full"
                  onChange={(e) => handleInputChange('userId', parseInt(e.target.value))}
                />
              </div>
            </div>
          </div>

          <div className="mt-8 gap-2 flex">
            <Button onClick={handleSave} variant="contained" color="primary" className="mr-2 pr-5">
              Save
            </Button>
            <Button onClick={() => navigate('/dispatches')} variant="outlined">
              Back
            </Button>
          </div>
        </>
      )}
    </div>
  );
};

export default NewDispatchPage;
