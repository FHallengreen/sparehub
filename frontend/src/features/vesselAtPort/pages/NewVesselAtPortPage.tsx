import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, TextField, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createVesselAtPort, getVesselsAtPorts } from '../../../api/vesselAtPortApi.ts';
import { VesselAtPortRequest } from '../../../interfaces/vesselAtPort.ts';
import { getPorts } from '../../../api/portApi.ts';
import { getVessels } from '../../../api/vesselApi.ts';

const NewVesselAtPortPage: React.FC = () => {
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [ports, setPorts] = useState<{ id: string; name: string }[]>([]);
    const [vessels, setVessels] = useState<{ id: string; name: string }[]>([]);
    const [vesselAtPort, setVesselAtPort] = useState<VesselAtPortRequest>({
        vesselId: '',
        portId: '',
        arrivalDate: '',
        departureDate: '',
    });

    useEffect(() => {
        const fetchVessels = async () => {
            try {
                const vesselsData = await getVessels();
                const vesselsAtPortData = await getVesselsAtPorts();

                console.log('Fetched vessels:', vesselsData);
                console.log('Fetched vessels at ports:', vesselsAtPortData);

                const vesselsAtPortIds = vesselsAtPortData.map(vesselAtPort => vesselAtPort.vessels[0].id.toString());
                const availableVessels = vesselsData.filter(vessel => !vesselsAtPortIds.includes(vessel.id.toString()));

                setVessels(availableVessels.map(vessel => ({ id: vessel.id.toString(), name: vessel.name })));
                console.log('Available vessels:', availableVessels);
            } catch (err) {
                console.error('Error fetching vessels:', err);
            }
        };

        fetchVessels();
    }, []);

    useEffect(() => {
        const fetchPorts = async () => {
            try {
                const data = await getPorts();
                setPorts(data.map(port => ({ id: port.id.toString(), name: port.name })));
                console.log('Fetched ports:', data);
            } catch (err) {
                console.error('Error fetching ports:', err);
            }
        };

        fetchPorts();
    }, []);

    const handleInputChange = (field: keyof VesselAtPortRequest, value: string) => {
        setVesselAtPort({ ...vesselAtPort, [field]: value });
    };

    const handleSave = async () => {
        if (!vesselAtPort.vesselId) {
            setError('Vessel must be selected.');
            return;
        }

        if (!vesselAtPort.portId) {
            setError('Port must be selected.');
            return;
        }

        setLoading(true);
        setError(null);

        console.log('Vessel at Port data being sent:', vesselAtPort);

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
                Add Vessel To Port
            </Typography>

            {error && <Typography color="error">{error}</Typography>}

            <TextField
                select
                label="Vessel"
                value={vesselAtPort.vesselId}
                onChange={(e) => handleInputChange('vesselId', e.target.value)}
                fullWidth
                className="mb-4"
                SelectProps={{ native: true }}
                InputLabelProps={{
                    shrink: true,
                }}
            >
                <option value="" disabled>Select a vessel</option>
                {vessels.map((vessel) => (
                    <option key={vessel.id} value={vessel.id}>
                        {vessel.name}
                    </option>
                ))}
            </TextField>

            <TextField
                select
                label="Port"
                value={vesselAtPort.portId}
                onChange={(e) => handleInputChange('portId', e.target.value)}
                fullWidth
                className="mb-4"
                SelectProps={{ native: true }}
                InputLabelProps={{
                    shrink: true,
                }}
            >
                <option value="" disabled>Select a port</option>
                {ports.map((port) => (
                    <option key={port.id} value={port.id}>
                        {port.name}
                    </option>
                ))}

            </TextField>
            <TextField
                label="Arrival Date"
                value={vesselAtPort.arrivalDate}
                type="date"
                onChange={(e) => handleInputChange('arrivalDate', e.target.value)}
                fullWidth
                className="mb-4"
                InputLabelProps={{
                    shrink: true,
                }}
            />
            <TextField
                label="Departure Date"
                value={vesselAtPort.departureDate}
                type="date"
                onChange={(e) => handleInputChange('departureDate', e.target.value)}
                fullWidth
                className="mb-4"
                InputLabelProps={{
                    shrink: true,
                }}
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