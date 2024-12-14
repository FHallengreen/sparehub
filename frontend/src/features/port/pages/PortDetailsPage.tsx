import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getPortById, updatePort, deletePort } from '../../../api/portApi';
import { PortDetail } from '../../../interfaces/port';

const PortDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();
  const [port, setPort] = useState<PortDetail | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPort = async () => {
      try {
        const data = await getPortById(id!);
        setPort(data);
      } catch (err) {
        console.error('Error fetching port:', err);
        setError('Failed to fetch port details.');
      } finally {
        setLoading(false);
      }
    };

    fetchPort();
  }, [id]);

  const handleInputChange = (field: keyof PortDetail, value: string) => {
    if (port) {
      setPort({ ...port, [field]: value });
    }
  };

  const handleSave = async () => {
    if (!port) return;

    try {
      const updatedPort = await updatePort(id!, port);
      setPort(updatedPort);
      showSnackbar('Port updated successfully!', 'success');
      navigate('/ports');
    } catch (err) {
      console.error('Error updating port:', err);
      showSnackbar('Failed to update port.', 'error');
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this port?')) {
      try {
        await deletePort(id!);
        showSnackbar('Port deleted successfully!', 'success');
        navigate('/ports');
      } catch (err) {
        console.error('Error deleting port:', err);
        showSnackbar('Failed to delete port.', 'error');
      }
    }
  };

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!port) return <Typography>No port found.</Typography>;

  return (
    <div className="container mx-auto p-6">
      <Typography variant="h4" className="text-2xl font-bold mb-6">
        Port Details
      </Typography>

      <TextField
        label="Port Name"
        value={port.name}
        onChange={(e) => handleInputChange('name', e.target.value)}
        fullWidth
        className="mb-4"
      />

      <div className="mt-8 gap-2 flex">
        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
          Save
        </Button>
        <Button onClick={handleDelete} variant="outlined" color="error">
          Delete
        </Button>
        <Button onClick={() => navigate('/ports')} variant="outlined">
          Back
        </Button>
      </div>
    </div>
  );
};

export default PortDetailsPage; 