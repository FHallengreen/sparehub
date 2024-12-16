import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createPort } from '../../../api/portApi';
import { PortRequest } from '../../../interfaces/port';
import PortDetailForm from '../components/PortDetailForm.tsx';

const NewPortPage: React.FC = () => {
  const navigate = useNavigate();
  const showSnackbar = useSnackbar();
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [port, setPort] = useState<PortRequest>({
    name: '',
    country: '',
    unlocode: '',
  });

  const handleInputChange = (field: keyof PortRequest, value: string) => {
    setPort({ ...port, [field]: value });
  };

  const handleSave = async () => {
    setLoading(true);
    setError(null);

    try {
      await createPort(port);
      showSnackbar('Port created successfully!', 'success');
      navigate('/ports');
    } catch (err) {
      console.error('Error creating port:', err);
      showSnackbar('Failed to create port.', 'error');
      setError('Failed to create port.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  return (
    <div className="container mx-auto p-6">
      
      {error && <Typography color="error">{error}</Typography>}

      <PortDetailForm
      title = "Create Port"
      fields = {[
        {label: 'Name', value: port.name, onChange: (value) => handleInputChange('name', value)},
      ]}
      />

      <div className="mt-8 gap-2 flex">
        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
          Save
        </Button>
        <Button onClick={() => navigate('/ports')} variant="outlined">
          Back
        </Button>
      </div>
    </div>
  );
};

export default NewPortPage; 