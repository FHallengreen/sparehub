import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getVessel, updateVessel, deleteVessel } from '../../../api/vesselApi';
import { VesselDetail } from '../../../interfaces/vessel';

const VesselDetailsPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [vessel, setVessel] = useState<VesselDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchVessel = async () => {
            try {
                const data = await getVessel(id!);
                setVessel(data);
            } catch (err) {
                console.error('Error fetching vessel:', err);
                setError('Failed to fetch vessel details.');
            } finally {
                setLoading(false);
            }
        };

        fetchVessel();
    }, [id]);

    const handleInputChange = (field: keyof VesselDetail, value: string) => {
        if (vessel) {
            setVessel({ ...vessel, [field]: value });
        }
    };

    const handleSave = async () => {
        if (!vessel) return;

        try {
            await updateVessel(id!, vessel);
            showSnackbar('Vessel updated successfully!', 'success');
            navigate('/vessels');
        } catch (err) {
            console.error('Error updating vessel:', err);
            showSnackbar('Failed to update vessel.', 'error');
        }
    };

    const handleDelete = async () => {
        if (window.confirm('Are you sure you want to delete this vessel?')) {
            try {
                await deleteVessel(id!);
                showSnackbar('Vessel deleted successfully!', 'success');
                navigate('/vessels');
            } catch (err) {
                console.error('Error deleting vessel:', err);
                showSnackbar('Failed to delete vessel.', 'error');
            }
        }
    };

    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;
    if (!vessel) return <Typography>No vessel found.</Typography>;

    return (
        <div className="container mx-auto p-6">
            <Typography variant="h4" className="text-2xl font-bold mb-6">
                Vessel Details
            </Typography>

            <TextField
                label="Vessel Name"
                value={vessel.name}
                onChange={(e) => handleInputChange('name', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Type"
                value={vessel.type}
                onChange={(e) => handleInputChange('type', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Owner ID"
                value={vessel.ownerId}
                onChange={(e) => handleInputChange('ownerId', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="IMO Number"
                value={vessel.imoNumber}
                onChange={(e) => handleInputChange('imoNumber', e.target.value)}
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
                <Button onClick={() => navigate('/vessels')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default VesselDetailsPage; 