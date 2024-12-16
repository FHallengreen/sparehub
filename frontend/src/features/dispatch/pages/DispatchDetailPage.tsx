import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField, IconButton } from '@mui/material';
import { Delete as DeleteIcon } from '@mui/icons-material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { DispatchDetail } from '../../../interfaces/dispatch.ts';
import DispatchDetailForm from '../components/DispatchDetailForm.tsx';
import { getDispatch, updateDispatch, deleteDispatch } from '../../../api/dispatchApi.ts';
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
        if (!id) return;
        const data = await getDispatch(id);
        setDispatch(data);
      } catch (err) {
        if (axios.isAxiosError(err) && err.response?.status === 404) {
          setError('Dispatch not found.');
        } else {
          setError('Failed to fetch dispatch.');
        }
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchData();
  }, [id]);

  const handleInputChange = (field: keyof DispatchDetail, value: string | number | null) => {
    setDispatch((prev) => prev ? { ...prev, [field]: value } : null);
  };

  const handleSave = async () => {
    if (!dispatch) return;
    try {
      await updateDispatch(dispatch.id.toString(), dispatch);
      showSnackbar('Dispatch updated successfully!', 'success');
    } catch {
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

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <div className="container mx-auto p-6">
      <Typography variant="h4" className="text-2xl font-bold mb-6">
        {dispatch?.id ? `Edit Dispatch: ID ${dispatch.id}` : 'Create New Dispatch'}
      </Typography>

      {error && <Typography color="error">{error}</Typography>}

      {dispatch && (
        <DispatchDetailForm
          title="Dispatch Details"
          fields={[
            {
              label: 'Origin Type',
              value: dispatch.originType,
              onChange: (value) => handleInputChange('originType', value),
              customComponent: (
                <TextField
                  label="Origin Type"
                  value={dispatch.originType}
                  onChange={(e) => handleInputChange('originType', e.target.value)}
                  variant="outlined"
                  fullWidth
                />
              ),
            },
            {
              label: 'Origin ID',
              value: dispatch.originId,
              onChange: (value) => handleInputChange('originId', value),
              customComponent: (
                <TextField
                  label="Origin ID"
                  type="number"
                  value={dispatch.originId || ''}
                  onChange={(e) => handleInputChange('originId', Number(e.target.value))}
                  variant="outlined"
                  fullWidth
                />
              ),
            },
            {
              label: 'Destination Type',
              value: dispatch.destinationType,
              onChange: (value) => handleInputChange('destinationType', value),
              customComponent: (
                <TextField
                  label="Destination Type"
                  value={dispatch.destinationType}
                  onChange={(e) => handleInputChange('destinationType', e.target.value)}
                  variant="outlined"
                  fullWidth
                />
              ),
            },
            {
              label: 'Destination ID',
              value: dispatch.destinationId,
              onChange: (value) => handleInputChange('destinationId', value),
              customComponent: (
                <TextField
                  label="Destination ID"
                  type="number"
                  value={dispatch.destinationId || ''}
                  onChange={(e) => handleInputChange('destinationId', Number(e.target.value))}
                  variant="outlined"
                  fullWidth
                />
              ),
            },
            {
              label: 'Transport Mode',
              value: dispatch.transportModeType,
              onChange: (value) => handleInputChange('transportModeType', value),
              customComponent: (
                <TextField
                  label="Transport Mode"
                  value={dispatch.transportModeType}
                  onChange={(e) => handleInputChange('transportModeType', e.target.value)}
                  variant="outlined"
                  fullWidth
                />
              ),
            },
          ]}
        />
      )}

      <div className="mt-8 gap-2 flex">
        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
          Save
        </Button>
        <Button onClick={() => navigate('/dispatches')} variant="outlined">
          Back
        </Button>
        {dispatch?.id && (
          <IconButton
            onClick={() => {
              if (window.confirm('Are you sure you want to delete this dispatch?')) {
                handleDelete();
              }
            }}
            color="error"
          >
            <DeleteIcon />
          </IconButton>
        )}
      </div>
    </div>
  );
};

export default DispatchDetailPage;
