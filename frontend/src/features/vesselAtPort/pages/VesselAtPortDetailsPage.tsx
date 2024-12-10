import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getVesselAtPort, updateVesselAtPort, deleteVesselAtPort } from '../../../api/vesselAtPortApi.ts';
import { getVesselById } from '../../../api/vesselApi.ts';
import { getPortById } from '../../../api/portApi.ts';
import { VesselAtPortDetail } from '../../../interfaces/vesselAtPort.ts';

const VesselAtPortDetailsPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [vesselAtPort, setVesselAtPort] = useState<VesselAtPortDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [vessel, setVessel] = useState<any>(null);
    const [port, setPort] = useState<any>(null);

    useEffect(() => {
        const fetchVesselAtPort = async () => {
            try {
                const data = await getVesselAtPort(id!);
                setVesselAtPort(data);
                const vesselData = await getVesselById(data.vesselId);
                const portData = await getPortById(data.portId);
                setVessel(vesselData);
                setPort(portData);
            } catch (err) {
                console.error('Error fetching vessel at port:', err);
                setError('Failed to fetch vessel at port details.');
            } finally {
                setLoading(false);
            }
        };

        fetchVesselAtPort();
    }, [id]);

    const handleInputChange = (field: keyof VesselAtPortDetail, value: string) => {
        if (vesselAtPort) {
            setVesselAtPort({ ...vesselAtPort, [field]: value });
        }
    };

    const handleSave = async () => {
        if (!vesselAtPort) return;

        try {
            await updateVesselAtPort(id!, vesselAtPort);
            showSnackbar('Vessel at Port updated successfully!', 'success');
            navigate('/vessels-at-ports');
        } catch (err) {
            console.error('Error updating vessel at port:', err);
            showSnackbar('Failed to update vessel at port.', 'error');
        }
    };

    const handleDelete = async () => {
        if (window.confirm('Are you sure you want to delete this vessel at port?')) {
            try {
                await deleteVesselAtPort(id!);
                showSnackbar('Vessel at Port deleted successfully!', 'success');
                navigate('/vessels-at-ports');
            } catch (err) {
                console.error('Error deleting vessel at port:', err);
                showSnackbar('Failed to delete vessel at port.', 'error');
            }
        }
    };

    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;
    if (!vesselAtPort) return <Typography>No vessel at port found.</Typography>;

    return (
        <div className="container mx-auto p-6">
            <Typography variant="h4" className="text-2xl font-bold mb-6">
                Vessel at Port Details
            </Typography>

            <TextField
                label="Vessel ID"
                value={vesselAtPort.vesselId}
                onChange={(e) => handleInputChange('vesselId', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Port ID"
                value={vesselAtPort.portId}
                onChange={(e) => handleInputChange('portId', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Arrival Date"
                value={vesselAtPort.arrivalDate}
                onChange={(e) => handleInputChange('arrivalDate', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Departure Date"
                value={vesselAtPort.departureDate}
                onChange={(e) => handleInputChange('departureDate', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Status"
                value={vesselAtPort.status}
                onChange={(e) => handleInputChange('status', e.target.value)}
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
                <Button onClick={() => navigate('/vessels-at-ports')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default VesselAtPortDetailsPage; 