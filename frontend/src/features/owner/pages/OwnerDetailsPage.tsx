import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext';
import OwnerDetailForm from '../components/OwnerDetailForm';
import { getOwner, updateOwner, deleteOwner } from '../../../api/ownerApi';

const OwnerDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [owner, setOwner] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const showSnackbar = useSnackbar();

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!id) {
          setLoading(false);
          return;
        }
        const data = await getOwner(id);
        setOwner(data);
      } catch (err) {
        console.error('Error fetching owner:', err);
        setError('Failed to fetch owner details.');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleInputChange = (field: string, value: string) => {
    setOwner({ ...owner, [field]: value });
  };

  const handleSave = async () => {
    try {
      await updateOwner(id!, owner);
      showSnackbar('Owner updated successfully!', 'success');
      navigate('/owners');
    } catch (err) {
      console.error('Error updating owner:', err);
      showSnackbar('Failed to update owner.', 'error');
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this owner?')) {
      try {
        await deleteOwner(id!);
        showSnackbar('Owner deleted successfully!', 'success');
        navigate('/owners');
      } catch (err) {
        console.error('Error deleting owner:', err);
        showSnackbar('Failed to delete owner.', 'error');
      }
    }
  };

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!owner) return <Typography>No owner found.</Typography>;

  return (
    <div className="container mx-auto p-6">
      <Typography variant="h4" className="text-2xl font-bold mb-6">
        Owner Details
      </Typography>

      <OwnerDetailForm
        title="Owner Information"
        fields={[
          { label: 'Name', value: owner.name, onChange: (value) => handleInputChange('name', value) },
        ]}
      />

      <div className="mt-8 flex gap-2">
        <Button onClick={handleSave} variant="contained" color="primary">
          Save
        </Button>
        <Button onClick={handleDelete} variant="outlined" color="error">
          Delete
        </Button>
        <Button onClick={() => navigate('/owners')} variant="outlined">
          Back
        </Button>
      </div>
    </div>
  );
};

export default OwnerDetailsPage;
