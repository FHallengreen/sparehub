import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, IconButton } from '@mui/material';
import { Delete as DeleteIcon } from '@mui/icons-material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { DispatchDetail } from '../../../interfaces/dispatch.ts';
import DispatchDetailForm from '../components/DispatchDetailForm.tsx';
import DispatchOrders from '../components/DispatchOrders.tsx';
import { getDispatch, createDispatch, updateDispatch, deleteDispatch } from '../../../api/dispatchApi.ts';
import axios from "axios";

const DispatchDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [dispatch, setDispatch] = useState<DispatchDetail | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const showSnackbar = useSnackbar();

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!id) {
          setLoading(false);
          return;
        } else {
          const data = await getDispatch(id);
          setDispatch(data);
        }
      }
      catch (err) {
        if (axios.isAxiosError(err) && err.response?.status === 404) {
          setError('Dispatch not found.');
        } else {
          setError('Failed to fetch dispatch.');
        }
      } finally {
        setLoading(false);
      }
    }
    if (id) {
      fetchData();
    }
  }, [id]);

  const handleInputChange = (field: keyof DispatchDetail, value: string | number | null) => {
    if (dispatch) {
      setDispatch({ ...dispatch, [field]: value });
    }
  };

  const handleSave = async () => {
    if (!dispatch) return;

    try {
      if (dispatch.id === 0) {
        await createDispatch(dispatch);
        showSnackbar('Dispatch created successfully!', 'success');
      } else {
        await updateDispatch(dispatch.id.toString(), dispatch);
        showSnackbar('Dispatch updated successfully!', 'success');
      }
    }
    catch {
      showSnackbar('Failed to save dispatch.', 'error');
    }
  };

  const handleDelete = async () => {
    if (!dispatch) return;
    try {
      await deleteDispatch(dispatch.id);
      showSnackbar('Dispatch deleted successfully!', 'success');
      navigate('/dispatches');
    } catch {
      showSnackbar('Failed to delete dispatch.', 'error');
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
                    if (window.confirm('Are you sure you want to delete this dispatch?')) {
                      handleDelete();
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
            <DispatchDetailForm
              title="ORIGIN DETAILS"
              fields={[
                { label: 'Origin Type', value: dispatch.originType, onChange: (value) => handleInputChange('originType', value) },
                { label: 'Origin ID', value: dispatch.originId, onChange: (value) => handleInputChange('originId', value), type: 'number' },
              ]}
            />
            <DispatchDetailForm
              title="DESTINATION DETAILS"
              fields={[
                { label: 'Destination Type', value: dispatch.destinationType, onChange: (value) => handleInputChange('destinationType', value) },
                { label: 'Destination ID', value: dispatch.destinationId, onChange: (value) => handleInputChange('destinationId', value) },
              ]}
            />
          </div>
          <DispatchDetailForm
            title="DISPATCH DETAILS"
            fields={[
              { label: 'Status', value: dispatch.dispatchStatus, onChange: (value) => handleInputChange('dispatchStatus', value) },
              { label: 'Transport Mode', value: dispatch.transportModeType, onChange: (value) => handleInputChange('transportModeType', value) },
              { label: 'Tracking Number', value: dispatch.trackingNumber, onChange: (value) => handleInputChange('trackingNumber', value) },
            ]}
          />
          <DispatchOrders orders={dispatch.orderNumbers} />
          <div className="mt-8 gap-2 flex">
            <Button onClick={handleSave} variant="contained" color="primary">
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

export default DispatchDetailPage;
