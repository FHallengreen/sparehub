import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, TextField, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createVesselAtPort } from '../../../api/vesselAtPortApi.ts';
import { VesselAtPortRequest } from '../../../interfaces/vesselAtPort.ts';

const NewVesselAtPortPage: React.FC = () => {
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [vesselAtPort, setVesselAtPort] = useState<VesselAtPortRequest>({
        vesselId: '',
        portId: '',
        arrivalDate: '',
        departureDate: '',
    });

    const handleInputChange = (field: keyof VesselAtPortRequest, value: string) => {
        setVesselAtPort({ ...vesselAtPort, [field]: value });
    };

    const handleSave = async () => {
        setLoading(true);
        setError(null);

        try {
            await createVesselAtPort(vesselAtPort);
            showSnackbar('Vessel at Port created successfully!', 'success');
            navigate('/vessels-at-ports');
        } catch (err) {
            console.error('Error creating vessel at port:', err);
            showSnackbar('Failed to create vessel at port.', 'error');
            setError('Failed to create vessel at port.');
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
                Create New Vessel at Port
            </Typography>

            {error && <Typography color="error">{error}</Typography>}

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

            <div className="mt-8 gap-2 flex">
                <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
                    Save
                </Button>
                <Button onClick={() => navigate('/vessels-at-ports')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default NewVesselAtPortPage; 