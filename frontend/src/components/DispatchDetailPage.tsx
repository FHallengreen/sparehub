import * as React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField, IconButton } from '@mui/material';
import axios from 'axios';
import { Delete as DeleteIcon } from '@mui/icons-material';
import { DispatchDetail } from '../interfaces/dispatch';
import { useSnackbar } from './SnackbarContext';

const API_URL = import.meta.env.VITE_API_URL;

const DispatchDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const isNewDispatch = id === 'new';
  const navigate = useNavigate();
  const [dispatch, setDispatch] = React.useState<DispatchDetail | null>(null);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const showSnackbar = useSnackbar();

  React.useEffect(() => {
    const fetchDispatch = async () => {
      try {
        if (!isNewDispatch) {
          const dispatchResponse = await axios.get<DispatchDetail>(
            `${API_URL}/api/dispatch/${id}`
          );
          setDispatch(dispatchResponse.data);
        } else {
          setDispatch({
            id: 0,
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
            orderNumbers: [],
          });
        }
      } catch (error) {
        if (axios.isAxiosError(error) && error.response?.status === 404) {
          showSnackbar('Dispatch not found.', 'error');
          navigate('/dispatches');
        } else {
          setError('Failed to fetch dispatch.');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchDispatch();
  }, [id, isNewDispatch]);

  const handleInputChange = (field: keyof DispatchDetail, value: string | number | null) => {
    if (dispatch) {
      setDispatch({ ...dispatch, [field]: value });
    }
  };

  const deleteDispatch = async (dispatchId: number) => {
    try {
      await axios.delete(`${API_URL}/api/dispatch/${dispatchId}`);
      showSnackbar('Dispatch deleted successfully!', 'success');
      navigate('/dispatches');
    } catch (err) {
      if (axios.isAxiosError(err) && err.response) {
        const { status } = err.response;
        if (status === 404) {
          showSnackbar('Dispatch not found.', 'error');
        } else {
          showSnackbar('Failed to delete dispatch.', 'error');
        }
      } else {
        showSnackbar('Unexpected error occurred.', 'error');
      }
    }
  };

  const handleSave = async () => {
    if (!dispatch) return;

    const sanitizedDispatch = { ...dispatch }; // Sanitize if needed

    try {
      if (isNewDispatch) {
        const response = await axios.post(
          `${API_URL}/api/dispatch`,
          sanitizedDispatch
        );
        setDispatch(response.data);
        showSnackbar('Dispatch created successfully!', 'success');
      } else {
        await axios.put(`${API_URL}/api/dispatch/${id}`, sanitizedDispatch);
        showSnackbar('Dispatch updated successfully!', 'success');
      }
    } catch (err) {
      showSnackbar('Failed to save dispatch.', 'error');
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <div className="container mx-auto p-6">
      {dispatch && (
        <>
          <div className="mt-8 gap-2 flex items-center">
            {dispatch.id !== 0 && (
              <>
                <Typography variant="h4" className="text-2xl font-bold mb-6 pb-4">
                  Dispatch ID: {dispatch.id}
                </Typography>
                <IconButton
                  onClick={() => {
                    if (window.confirm("Are you sure you want to delete this dispatch?")) {
                      deleteDispatch(dispatch.id);
                    }
                  }}
                  className="text-red-500"
                >
                  <DeleteIcon />
                </IconButton>
              </>
            )}
          </div>

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

          <div className="shadow-lg p-6 rounded-md bg-white">
            <Typography variant="h5" className="font-bold mb-4 pb-4">RELATED ORDERS:</Typography>
            <div className="flex flex-wrap gap-2">
              {dispatch.orderNumbers && dispatch.orderNumbers.length > 0 ? (
                dispatch.orderNumbers.map((orderNumber, index) => (
                  <Typography
                    key={index}
                    variant="body1"
                    className="bg-gray-100 px-4 py-2 rounded-md"
                  >
                    {orderNumber}
                  </Typography>
                ))
              ) : (
                <Typography variant="body2">No related orders.</Typography>
              )}
            </div>
          </div>

          <div className="mt-8 gap-2 flex">
            <Button onClick={handleSave} variant="contained" color="primary" className="mr-2 pr-5">Save</Button>
            <Button onClick={() => navigate('/dispatches')} variant="outlined">Back</Button>
          </div>
        </>
      )}
    </div>
  );
};

export default DispatchDetailPage;
